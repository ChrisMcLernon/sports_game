using sports_game.src.Handlers;
using sports_game.src.Services;

namespace sports_game
{
    static public class Program
    {
        static private void Main()
        {
            GameHandler gameHandler = new();

            gameHandler.GameLoop();

            
        }
    }
}
