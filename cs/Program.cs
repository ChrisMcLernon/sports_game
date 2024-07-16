using sports_game.src.Services;
using sports_game.src.Models;
using sports_game.src.Handlers;

namespace sports_game
{
    public class Program
    {
        public static void Main()
        {
            List<Person> totalAvailablePlayers = JsonReader.Read<List<Person>>("Player_Stats.json");
            List<Person> totalAvailableStaff = JsonReader.Read<List<Person>>("Staff_Stats.json");
            MarketHandler marketHandler = new();

            GameHandler gameHandler = new(totalAvailablePlayers, totalAvailableStaff, marketHandler);

            gameHandler.StartGame();

            foreach (var p in gameHandler.AvailablePlayers)
            {
                Console.WriteLine($"Player: {p.Name}, Age: {p.Age}, Value: {p.Value}, " +
                              $"Position: {p.CurrentPosition.Name}, Modifier: {p.CurrentPosition.Modifier}, " +
                              $"Size: {p.CurrentPosition.Size}, Cost: {p.Cost}, Status: {p.Status}");

                Console.WriteLine("Effects:");
                foreach (var effect in p.Effects)
                {
                    Console.WriteLine($"- {effect.Name}: {effect.Description} (+{effect.Value})");
                }
            }

            foreach (var s in gameHandler.AvailableStaff)
            {
                Console.WriteLine($"Staff: {s.Name}, Age: {s.Age}, Value: {s.Value}, " +
                              $"Position: {s.CurrentPosition.Name}, Modifier: {s.CurrentPosition.Modifier}, " +
                              $"Size: {s.CurrentPosition.Size}, Cost: {s.Cost}, Status: {s.Status}");
                
                Console.WriteLine("Effects:");
                foreach (var effect in s.Effects)
                {
                    Console.WriteLine($"- {effect.Name}: {effect.Description} (+{effect.Value})");
                }
            }
        }
    }
}
