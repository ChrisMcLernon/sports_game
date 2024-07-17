using System.Text;
using sports_game.src.Entities;
using sports_game.src.Models;
using sports_game.src.Services;

namespace sports_game.src.Handlers
{
    public class GameHandler()
    {
        public Team? PlayerTeam { get; set; }
        public Team? OpponentTeam { get; set; }
        public List<Person> AvailablePlayers { get; set; } = [];
        public List<Person> AvailableStaff { get; set; } = [];
        public static List<string> TeamNames { get; set; }
        public MarketHandler MarketHandler { get; set; }
        static public Random? SetRandom { get; set; }
        public int TeamSize { get; set; } = 5;
        public int StaffSize { get; set; } = 3;

        public void GenerateStarterTeam()
        {
            AvailablePlayers = [.. AvailablePlayers.OrderBy(x => SetRandom.Next())];
            string teamName = ReadText("Enter Team Name: ");
            PlayerTeam = new Team(teamName);
            OpponentTeam = new Team(GenerateRandomString());
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

        public static string GenerateRandomString()
        {
            return TeamNames[SetRandom.Next(TeamNames.Count)];
        }

        public static string ReadText(string prompt = "")
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                
                if (input == "")
                {
                    Console.WriteLine("No input is invalid");
                }
                else
                {
                    return input;
                }
            }
        }

        public void CalculatePositionScore()
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

        public static void Close()
        {
            Console.WriteLine("Goodbye!");
            Environment.Exit(0);
        }

        public static void ConfigSeed(string seed)
        {
            int calculatedSeed = 0;
            foreach (char c in seed)
            {
                calculatedSeed += Convert.ToInt32(c);
            }
            SetRandom = new Random(calculatedSeed);
        }

        public static void SetSeed()
        {
            Console.Write("Enter Seed: ");
            string seed = ReadText();
            ConfigSeed(seed);
        }

        public  void StartGame()
        {
            Console.WriteLine("Welcome to '_' (0 to exit | 1 to start game)");
            string input = ReadText();
            if (input == "0")
            {
                Close();
            }
            else if (input == "1")
            {
                InitializeData(this);
                SetSeed();
                GenerateStarterTeam();
            }
        }

        private static void InitializeData(GameHandler gameHandler)
        {
            gameHandler.AvailablePlayers = JsonReader.Read<List<Person>>("Football_Player_Stats");
            gameHandler.AvailableStaff = JsonReader.Read<List<Person>>("Football_Staff_Stats");
            TeamNames = JsonReader.Read<List<string>>("Team_Names");
        }

        public void PlayGame()
        {
        }

        public void GameLoop()
        {
            StartGame();
            PlayGame();
        }
    }
}