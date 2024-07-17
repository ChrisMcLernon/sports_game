using System.Text;
using sports_game.src.Entities;
using sports_game.src.Models;
using sports_game.src.Services;

namespace sports_game.src.Handlers
{
    public class GameHandler()
    {
        static private Team? PlayerTeam { get; set; }
        static private Team? OpponentTeam { get; set; }
        static private List<Person> AvailablePlayers { get; set; } = [];
        static private List<Person> AvailableStaff { get; set; } = [];
        static private List<string>? TeamNames { get; set; }
        static private MarketHandler? MarketHandlerLocal { get; set; }
        static private EditorHandler? EditorHandlerLocal { get; set; }
        static private Random? SetRandom { get; set; }
        static private int TeamSize { get; set; } = 2;
        static private int StaffSize { get; set; } = 3;

        static private string ReadText(string prompt = "")
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                
                if (input == "" || input == null)
                {
                    Console.WriteLine("No input is invalid");
                }
                else
                {
                    return input;
                }
            }
        }

        static private void AddAvailablePerson(Person person)
        {
            if (PlayerTeam == null || OpponentTeam == null || AvailablePlayers == null || AvailableStaff == null)
            {
                throw new Exception("Some Data not Initialized");
            }
            if (PlayerTeam.PossiblePlayerPositions.Contains(person.CurrentPosition.Name))
            {
                AvailablePlayers.Add(person);
            }
            else
            {
                AvailableStaff.Add(person);
            }
        }

        static private void RemoveAvailablePerson(Person person)
        {
            if (PlayerTeam == null || OpponentTeam == null || AvailablePlayers == null || AvailableStaff == null)
            {
                throw new Exception("Some Data not Initialized");
            }
            if (PlayerTeam.PossiblePlayerPositions.Contains(person.CurrentPosition.Name))
            {
                AvailablePlayers.Remove(person);
            }
            else
            {
                AvailableStaff.Remove(person);
            }
        }

        static private void Close()
        {
            Console.WriteLine("Goodbye!");
            Environment.Exit(0);
        }

        static private void ConfigSeed(string seed)
        {
            int calculatedSeed = 0;
            foreach (char c in seed)
            {
                calculatedSeed += Convert.ToInt32(c);
            }
            SetRandom = new Random(calculatedSeed);
        }

        static private void SetSeed()
        {
            Console.Write("Enter Seed: ");
            string seed = ReadText();
            ConfigSeed(seed);
        }

        static private List<int> CalculateScore()
        {
            List<int> totalPoints = [0, 0];
            double unroundedPlayerValue = 0;
            double unroundedEnemyValue = 0;


            if (PlayerTeam != null && OpponentTeam != null && PlayerTeam.Players != null && OpponentTeam.Players != null) 
            {
                foreach (var player in PlayerTeam.Players)
                {
                    unroundedPlayerValue += player.Value;

                    Console.WriteLine(player.Name + " | " + player.CurrentPosition.Name + " | " + player.CurrentPosition.Modifier + " | " + unroundedPlayerValue);

                    unroundedPlayerValue += PlayerTeam.EffectHandlerTeam.ApplyPersonEffects(player);
                    unroundedPlayerValue *= player.CurrentPosition.Modifier;

                    Console.WriteLine(player.Name + " | " + player.CurrentPosition.Name + " | " + player.CurrentPosition.Modifier + " | " + unroundedPlayerValue);

                    totalPoints[0] += Convert.ToInt32(Math.Round(unroundedPlayerValue));
                    unroundedPlayerValue = 0;
                }
                foreach (var enemy in OpponentTeam.Players)
                {
                    unroundedEnemyValue += enemy.Value;

                    Console.WriteLine(enemy.Name + " | " + enemy.CurrentPosition.Name + " | " + enemy.CurrentPosition.Modifier + " | " + unroundedEnemyValue);

                    unroundedEnemyValue += OpponentTeam.EffectHandlerTeam.ApplyPersonEffects(enemy);
                    unroundedEnemyValue *= enemy.CurrentPosition.Modifier;

                    Console.WriteLine(enemy.Name + " | " + enemy.CurrentPosition.Name + " | " + enemy.CurrentPosition.Modifier + " | " + unroundedEnemyValue);

                    totalPoints[1] += Convert.ToInt32(Math.Round(unroundedEnemyValue));
                    unroundedEnemyValue = 0;

                }

                return totalPoints;
            }
            throw new Exception("Some Data not Initialized");
        }

        static private void GenerateOpponentTeam()
        {
            
            if (SetRandom == null || TeamNames == null || OpponentTeam == null)
            {
                throw new Exception("Some Data not Initialized");
            }

            OpponentTeam = new Team(TeamNames[SetRandom.Next(TeamNames.Count)]);
            OpponentTeam.GeneratePossiblePositions();

            while (OpponentTeam.Players.Count < TeamSize)
            {
                Person player = AvailablePlayers[SetRandom.Next(AvailablePlayers.Count)];
                OpponentTeam.AddPerson(player);
                Console.WriteLine(player.Name + " | " + player.CurrentPosition.Name + " | " + player.CurrentPosition.Modifier + " | " + player.Value);
                if (OpponentTeam.Players.Contains(player))
                {
                    RemoveAvailablePerson(player);
                    Console.WriteLine("Removed -> " + player.Name + " | " + player.CurrentPosition.Name + " | " + player.CurrentPosition.Modifier + " | " + player.Value);
                }
                else if (OpponentTeam.Players.Count == 0)
                {
                    Console.WriteLine("No Players Added to Opponent Team");
                }
            }

            for (int i = 0; i < AvailableStaff.Count; i++)
            {
                while (OpponentTeam.Staff.Count < StaffSize)
                {
                    OpponentTeam.AddPerson(AvailableStaff[SetRandom.Next(AvailableStaff.Count)]);
                }
            }
        }

        static private void GenerateStarterTeam()
        {
            if (SetRandom == null || TeamNames == null)
            {
                throw new Exception("Some Data not Initialized");
            }

            AvailablePlayers = [.. AvailablePlayers.OrderBy(x => SetRandom.Next())];
            string teamName = ReadText("Enter Team Name: ");
            PlayerTeam = new Team(teamName);
            OpponentTeam = new Team(TeamNames[SetRandom.Next(TeamNames.Count)]);
            PlayerTeam.GeneratePossiblePositions();

            for (int i = 0; i < AvailablePlayers.Count; i++)
            {
                while (PlayerTeam.Players.Count < TeamSize)
                {
                    Person player = AvailablePlayers[SetRandom.Next(AvailablePlayers.Count)];
                    PlayerTeam.AddPerson(player);
                    Console.WriteLine(player.Name + " | " + player.CurrentPosition.Name + " | " + player.CurrentPosition.Modifier + " | " + player.Value);
                    if (PlayerTeam.Players.Contains(player))
                    {
                        RemoveAvailablePerson(player);
                        Console.WriteLine("Removed -> " + player.Name + " | " + player.CurrentPosition.Name + " | " + player.CurrentPosition.Modifier + " | " + player.Value);
                    }
                    
                }
            }

            for (int i = 0; i < AvailableStaff.Count; i++)
            {
                while (PlayerTeam.Staff.Count < StaffSize)
                {
                    PlayerTeam.AddPerson(AvailableStaff[SetRandom.Next(AvailableStaff.Count)]);
                }
            }
        }

        static private void InitializeData()
        {
            AvailablePlayers = JsonReader.Read<List<Person>>("Football_Player_Stats");
            AvailableStaff = JsonReader.Read<List<Person>>("Football_Staff_Stats");
            TeamNames = JsonReader.Read<List<string>>("Team_Names");
            MarketHandlerLocal = new(AvailablePlayers, AvailableStaff);
            EditorHandlerLocal = new();
        }

        static private void StartGame()
        {
            Console.WriteLine("Welcome to '_' (0 to exit | 1 to start game)");
            string input = ReadText();
            if (input == "0")
            {
                Close();
            }
            else if (input == "1")
            {
                InitializeData();
                SetSeed();
                GenerateStarterTeam();
            }
        }

        static private void PlayMatch()
        {
            GenerateOpponentTeam();

            if (PlayerTeam == null || OpponentTeam == null)
            {
                throw new Exception("Some Data not Initialized");
            }

            List<int> score = CalculateScore();
            int playerScore = score[0];
            int opponentScore = score[1];

            if (playerScore > opponentScore)
            {
                Console.WriteLine("You Win!");
                Console.WriteLine($"{PlayerTeam.Name}: {playerScore} - {OpponentTeam.Name}: {opponentScore}");
            }
            else if (playerScore < opponentScore)
            {
                Console.WriteLine("You Lose!");
                Console.WriteLine($"{PlayerTeam.Name}: {playerScore} - {OpponentTeam.Name}: {opponentScore}");
                Close();
            }
            else
            {
                Console.WriteLine("Draw!");
                Console.WriteLine($"{PlayerTeam.Name}: {playerScore} - {OpponentTeam.Name}: {opponentScore}");
                Close();
            }

            HandleOpponentTeam();
        }

        private static void HandleOpponentTeam()
        {
            if (OpponentTeam is null)
            {
                throw new Exception("Some Data not Initialized");
            }
            
            foreach (var player in OpponentTeam.Players)
            {
                AddAvailablePerson(player);
            }
        }

        static private void PlayGame()
        {
            while (true)
            {
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1. Team Editor");
                Console.WriteLine("2. Continue to Next Match");
                Console.WriteLine("0. Exit");
                string input = ReadText();

                switch (input)
                {
                    case "0":
                        Close();
                        break;
                    case "1":
                        Console.WriteLine("Editing Team");
                        if (PlayerTeam is null){
                            throw new Exception("Some Data not Initialized");
                        }
                        foreach (var player in PlayerTeam.Players)
                        {
                            Console.WriteLine($"{player.Name} | {player.CurrentPosition.Name} | {player.CurrentPosition.Modifier} | {player.Value}");
                        }
                        break;
                    case "2":
                        Console.WriteLine("Playing Match");
                        PlayMatch();
                        break;
                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }
            }
        }

        public void GameLoop()
        {
            StartGame();
            PlayGame();
        }
    }
}