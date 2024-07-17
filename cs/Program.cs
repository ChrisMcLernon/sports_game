using sports_game.src.Services;
using sports_game.src.Models;
using sports_game.src.Handlers;

namespace sports_game
{
    public class Program
    {
        public static void Main()
        {
            List<Person> totalAvailablePlayers = JsonReader.Read<List<Person>>("Football_Player_Stats");
            List<Person> totalAvailableStaff = JsonReader.Read<List<Person>>("Football_Staff_Stats");
            MarketHandler marketHandler = new();

            GameHandler gameHandler = new(totalAvailablePlayers, totalAvailableStaff, marketHandler);

            gameHandler.GameLoop();
        }
    }
}
