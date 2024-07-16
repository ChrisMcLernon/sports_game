using sports_game.src.Entities;
using sports_game.src.Models;

namespace sports_game.src.Handlers
{
    public class GameHandler(List<Person> availablePlayers, List<Person> availableStaff, MarketHandler marketHandler, Team? playerTeam = null, Team? opponentTeam = null)
    {
        public Team? PlayerTeam { get; set; } = playerTeam;
        public Team? OpponentTeam { get; set; } = opponentTeam;
        public List<Person> AvailablePlayers { get; set; } = availablePlayers;
        public List<Person> AvailableStaff { get; set; } = availableStaff;
        public MarketHandler MarketHandler { get; set; } = marketHandler;
        public Random? Random { get; set; }

        public static string ReadText()
        {
            while (true)
            {
                string? input = Console.ReadLine();
                
                if (input == null)
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

        public void ConfigSeed(string seed)
        {
            int calculatedSeed = 0;
            foreach (char c in seed)
            {
                calculatedSeed += Convert.ToInt32(c);
            }
            Random = new Random(calculatedSeed);
        }

        public void SetSeed()
        {
            Console.Write("Enter Seed: ");
            string seed = ReadText();
            ConfigSeed(seed);
        }

        public void StartGame()
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
            // Implementation for playing the game
        }

        public void GameLoop()
        {
            StartGame();
            PlayGame();
        }
    }
}