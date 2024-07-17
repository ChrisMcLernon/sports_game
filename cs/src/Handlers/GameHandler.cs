using System.Text;
using sports_game.src.Entities;
using sports_game.src.Models;
using sports_game.src.Services;

namespace sports_game.src.Handlers
{
    public class GameHandler(List<Person> availablePlayers, List<Person> availableStaff, MarketHandler marketHandler)
    {
        public Team? PlayerTeam { get; set; }
        public Team? OpponentTeam { get; set; }
        public List<Person> AvailablePlayers { get; set; } = availablePlayers;
        public List<Person> AvailableStaff { get; set; } = availableStaff;
        public MarketHandler MarketHandler { get; set; } = marketHandler;
        static public Random? SetRandom { get; set; }

        public void GenerateStarterTeam(){
            AvailablePlayers = [.. AvailablePlayers.OrderBy(x => SetRandom.Next())];
            string teamName = ReadText();
            PlayerTeam = new Team(teamName);
            OpponentTeam = new Team(GenerateRandomString());
            PlayerTeam.GeneratePossiblePositions();
            OpponentTeam.GeneratePossiblePositions();

            for (int i = 0; i < AvailablePlayers.Count; i++)
            {
                while (PlayerTeam.Players.Count < 3)
                {
                    PlayerTeam.AddPerson(AvailablePlayers[SetRandom.Next(AvailablePlayers.Count)]);
                }
                while (OpponentTeam.Players.Count < 3)
                {
                    OpponentTeam.AddPerson(AvailablePlayers[SetRandom.Next(AvailablePlayers.Count)]);
                }
            }
        }

        public static string GenerateRandomString()
        {
            List<string> Names = JsonReader.Read<List<string>>("Team_Names.json");
            return Names[SetRandom.Next(Names.Count)];
        }

        public static string ReadText()
        {
            while (true)
            {
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
                            double unroundedValue = player.Value * player.CurrentPosition.Modifier;


                            int roundedValue = (int)Math.Floor(unroundedValue);
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

        public static void StartGame()
        {
            Console.WriteLine("Welcome to '_' (0 to exit | 1 to start game)");
            string input = ReadText();
            if (input == "0")
            {
                Close();
            }
            else if (input == "1")
            {
                SetSeed();
            }
        }

        public void PlayGame()
        {
            GenerateStarterTeam();
        }

        public void GameLoop()
        {
            StartGame();
            PlayGame();
        }
    }
}