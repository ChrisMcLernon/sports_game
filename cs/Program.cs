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

            gameHandler.GameLoop();

            Console.WriteLine($"Player Team: {gameHandler.PlayerTeam.Name}");

            foreach (var p in gameHandler.PlayerTeam.Players)
            {
                Console.WriteLine($"Player: {p.Name}, Age: {p.Age}, Value: {p.Value}, " +
                              $"Position: {p.CurrentPosition.Name}, Modifier: {p.CurrentPosition.Modifier}, " +
                              $"Size: {p.CurrentPosition.Size}, Cost: {p.Cost}, Status: {p.Status}");

                Console.WriteLine("Effects:");
                foreach (var effect in gameHandler.PlayerTeam.EffectHandlerTeam.Effects[p.CurrentPosition.Name])
                {
                    Console.WriteLine($"- {effect.Name}: {effect.Description} (+{effect.Value})");
                }
            }

            Console.WriteLine($"Opponent Team: {gameHandler.OpponentTeam.Name}");
            foreach (var p in gameHandler.OpponentTeam.Players)
            {
                Console.WriteLine($"Player: {p.Name}, Age: {p.Age}, Value: {p.Value}, " +
                              $"Position: {p.CurrentPosition.Name}, Modifier: {p.CurrentPosition.Modifier}, " +
                              $"Size: {p.CurrentPosition.Size}, Cost: {p.Cost}, Status: {p.Status}");
                
                Console.WriteLine("Effects:");
                foreach (var effect in gameHandler.OpponentTeam.EffectHandlerTeam.Effects[p.CurrentPosition.Name])
                {
                    Console.WriteLine($"- {effect.Name}: {effect.Description} (+{effect.Value})");
                }
            }

            gameHandler.PlayerTeam.ReplacePlayer(gameHandler.PlayerTeam.Players[0], gameHandler.AvailablePlayers[0]);

            Console.WriteLine($"Player Team: {gameHandler.PlayerTeam.Name}");

            foreach (var p in gameHandler.PlayerTeam.Players)
            {
                Console.WriteLine($"Player: {p.Name}, Age: {p.Age}, Value: {p.Value}, " +
                              $"Position: {p.CurrentPosition.Name}, Modifier: {p.CurrentPosition.Modifier}, " +
                              $"Size: {p.CurrentPosition.Size}, Cost: {p.Cost}, Status: {p.Status}");

                Console.WriteLine("Effects:");
                foreach (var effect in gameHandler.PlayerTeam.EffectHandlerTeam.Effects[p.CurrentPosition.Name])
                {
                    Console.WriteLine($"- {effect.Name}: {effect.Description} (+{effect.Value})");
                }
            }

            Console.WriteLine($"Opponent Team: {gameHandler.OpponentTeam.Name}");
            foreach (var p in gameHandler.OpponentTeam.Players)
            {
                Console.WriteLine($"Player: {p.Name}, Age: {p.Age}, Value: {p.Value}, " +
                              $"Position: {p.CurrentPosition.Name}, Modifier: {p.CurrentPosition.Modifier}, " +
                              $"Size: {p.CurrentPosition.Size}, Cost: {p.Cost}, Status: {p.Status}");
                
                Console.WriteLine("Effects:");
                foreach (var effect in gameHandler.OpponentTeam.EffectHandlerTeam.Effects[p.CurrentPosition.Name])
                {
                    Console.WriteLine($"- {effect.Name}: {effect.Description} (+{effect.Value})");
                }
            }
        }
    }
}
