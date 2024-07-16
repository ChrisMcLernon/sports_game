using System.Text;
using sports_game.src.Entities;
using sports_game.src.Models;

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
            OpponentTeam = new Team(GenerateRandomString(5));
            PlayerTeam.GeneratePossiblePositions();
            OpponentTeam.GeneratePossiblePositions();

            foreach (var player in AvailablePlayers)
            {
                if (PlayerTeam.Players.Count == 0)
                {
                    PlayerTeam.AddPerson(player);
                }
                else
                {
                    OpponentTeam.AddPerson(player);
                }
            }
        }

        public static string GenerateRandomString(int length)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            StringBuilder result = new(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(letters[SetRandom.Next(letters.Length)]);
            }
            return result.ToString();
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