using sports_game.src.Handlers;

namespace sports_game
{
    public class Program
    {
        public static void Main()
        {
            GameHandler gameHandler = new();

            gameHandler.GameLoop();
        }
    }
}
