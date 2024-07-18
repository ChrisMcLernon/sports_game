using System.ComponentModel;
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
        static private int TeamSize { get; } = 5;
        static private int StaffSize { get; } = 3;

        public void GameLoop()
        {
            StartGame();
            PlayGame();
        }

        static private void StartGame()
        {
            Console.WriteLine("Welcome to '_' (0 to exit | 1 to start game)");
            string input = InputReader.ReadText();
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

        static private void InitializeData()
        {
            AvailablePlayers = JsonReader.Read<List<Person>>("Football_Player_Stats");
            AvailableStaff = JsonReader.Read<List<Person>>("Football_Staff_Stats");
            TeamNames = JsonReader.Read<List<string>>("Team_Names");
            MarketHandlerLocal = new(AvailablePlayers, AvailableStaff);
            EditorHandlerLocal = new();
        }

        static private void SetSeed()
        {
            Console.Write("Enter Seed: ");
            string seed = InputReader.ReadText();
            ConfigSeed(seed);
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

        static private void GenerateStarterTeam()
        {
            if (SetRandom is null || TeamNames is null)
            {
                throw new Exception("Some Data not Initialized");
            }

            AvailablePlayers = [.. AvailablePlayers.OrderBy(x => SetRandom.Next())];
            string teamName = InputReader.ReadText("Enter Team Name: ");
            PlayerTeam = new Team(teamName);
            PlayerTeam.GeneratePossiblePositions();

            while (PlayerTeam.Players.Count < TeamSize && AvailablePlayers.Count > 0)
            {
                Person player = AvailablePlayers[SetRandom.Next(AvailablePlayers.Count)];
                if (!PlayerTeam.PositionFilled(player.CurrentPosition))
                {
                    PlayerTeam.AddPerson(player);
                    AvailablePlayers.Remove(player);
                }

            }

            while (PlayerTeam.Staff.Count < StaffSize && AvailableStaff.Count > 0)
            {
                Person staff = AvailableStaff[SetRandom.Next(AvailableStaff.Count)];
                if (!PlayerTeam.PositionFilled(staff.CurrentPosition))
                {
                    PlayerTeam.AddPerson(staff);
                    AvailableStaff.Remove(staff);
                }
            }
        }

        static private void PlayGame()
        {
            if (MarketHandlerLocal is null || PlayerTeam is null)
            {
                throw new Exception("Some Data not Initialized");
            }

            while (true)
            {
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1. Team Editor");
                Console.WriteLine("2. Continue to Next Match");
                Console.WriteLine("3. Market");
                Console.WriteLine("0. Exit");
                string input = InputReader.ReadText();

                switch (input)
                {
                    case "0":
                        Close();
                        break;
                    case "1":
                        Console.WriteLine("Editing Team");
                        foreach (var player in PlayerTeam.Players)
                        {
                            Console.WriteLine($"{player.Name} | {player.CurrentPosition.Name} | {player.CurrentPosition.Modifier} | {player.Value}");
                        }
                        break;
                    case "2":
                        Console.WriteLine("Playing Match");
                        PlayMatch();
                        break;
                    case "3":
                        MarketHandlerLocal.MarketMenu();
                        break;
                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }
            }
        }

        static private void PlayMatch()
        {
            GenerateOpponentTeam();

            if (PlayerTeam is null || OpponentTeam is null)
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

        static private void GenerateOpponentTeam()
        {
            if (SetRandom is null || TeamNames is null)
            {
                throw new Exception("Some Data not Initialized");
            }

            OpponentTeam = new Team(TeamNames[SetRandom.Next(TeamNames.Count)]);
            OpponentTeam.GeneratePossiblePositions();

            while (OpponentTeam.Players.Count < TeamSize && AvailablePlayers.Count > 0)
            {
                Person player = AvailablePlayers[SetRandom.Next(AvailablePlayers.Count)];
                if (!OpponentTeam.PositionFilled(player.CurrentPosition))
                {
                    OpponentTeam.AddPerson(player);
                    RemoveAvailablePerson(player);
                }
            }

            while (OpponentTeam.Staff.Count < StaffSize && AvailableStaff.Count > 0)
            {
                Person staff = AvailableStaff[SetRandom.Next(AvailableStaff.Count)];
                if (!OpponentTeam.PositionFilled(staff.CurrentPosition))
                {
                    OpponentTeam.AddPerson(staff);
                    RemoveAvailablePerson(staff);
                }
            }

        }

        static private void HandleOpponentTeam()
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

        static private void Close()
        {
            Console.WriteLine("Goodbye!");
            Environment.Exit(0);
        }

        static private void AddAvailablePerson(Person person)
        {
            if (PlayerTeam is null || OpponentTeam is null)
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
            if (PlayerTeam is null || OpponentTeam is null)
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

        static private List<int> CalculateScore()
        {
            List<int> totalPoints = [0, 0];
            double unroundedPlayerValue = 0;
            double unroundedEnemyValue = 0;

            if (PlayerTeam is null || OpponentTeam is null)
            {
                throw new Exception("Some Data not Initialized");
            }

            foreach (var player in PlayerTeam.Players)
            {
                unroundedPlayerValue += player.Value;

                unroundedPlayerValue += PlayerTeam.EffectHandlerTeam.ApplyPersonEffects(player);
                unroundedPlayerValue *= player.CurrentPosition.Modifier;

                totalPoints[0] += Convert.ToInt32(Math.Round(unroundedPlayerValue));
                unroundedPlayerValue = 0;
            }
            foreach (var enemy in OpponentTeam.Players)
            {
                unroundedEnemyValue += enemy.Value;

                unroundedEnemyValue += OpponentTeam.EffectHandlerTeam.ApplyPersonEffects(enemy);
                unroundedEnemyValue *= enemy.CurrentPosition.Modifier;

                totalPoints[1] += Convert.ToInt32(Math.Round(unroundedEnemyValue));
                unroundedEnemyValue = 0;

            }

            return totalPoints;
        }
    }
}