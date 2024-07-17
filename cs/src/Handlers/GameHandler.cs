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
        static private MarketHandler? MarketHandler { get; set; }
        static private Random? SetRandom { get; set; }
        static private int TeamSize { get; set; } = 5;
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

        static private void CalculatePositionScore()
        {
            if (PlayerTeam != null && OpponentTeam != null && PlayerTeam.Players != null && OpponentTeam.Players != null) 
            {
                foreach (var player in PlayerTeam.Players)
                {
                    foreach (var enemy in OpponentTeam.Players)
                    {
                        if (player.CurrentPosition.Name == enemy.CurrentPosition.Name)
                        {
                            double unroundedPlayerValue = player.Value * player.CurrentPosition.Modifier;
                            

                            double unroundedEnemyValue = enemy.Value * enemy.CurrentPosition.Modifier;
                        }
                    }
                }
            }
            else
            {
                throw new Exception("PlayerTeam or OpponentTeam is null");
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
            OpponentTeam.GeneratePossiblePositions();

            for (int i = 0; i < AvailablePlayers.Count; i++)
            {
                while (PlayerTeam.Players.Count < TeamSize)
                {
                    PlayerTeam.AddPerson(AvailablePlayers[SetRandom.Next(AvailablePlayers.Count)]);
                }
                while (OpponentTeam.Players.Count < TeamSize)
                {
                    OpponentTeam.AddPerson(AvailablePlayers[SetRandom.Next(AvailablePlayers.Count)]);
                }
            }

            for (int i = 0; i < AvailableStaff.Count; i++)
            {
                while (PlayerTeam.Staff.Count < StaffSize)
                {
                    PlayerTeam.AddPerson(AvailableStaff[SetRandom.Next(AvailableStaff.Count)]);
                }
                while (OpponentTeam.Staff.Count < StaffSize)
                {
                    OpponentTeam.AddPerson(AvailableStaff[SetRandom.Next(AvailableStaff.Count)]);
                }
            }
        }

        static private void InitializeData()
        {
            AvailablePlayers = JsonReader.Read<List<Person>>("Football_Player_Stats");
            AvailableStaff = JsonReader.Read<List<Person>>("Football_Staff_Stats");
            TeamNames = JsonReader.Read<List<string>>("Team_Names");
            MarketHandler = new(AvailablePlayers, AvailableStaff);
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

        static private void PlayGame()
        {
        }

        public void GameLoop()
        {
            StartGame();
            PlayGame();
        }
    }
}