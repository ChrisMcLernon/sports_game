using sports_game.src.Models;
using sports_game.src.Services;

namespace sports_game.src.Handlers
{
    public class MarketHandler(List<Person> availablePlayers, List<Person> availableStaff)
    {
        private List<Person> AvailablePlayers { get; set;} = availablePlayers;
        private List<Person> AvailableStaff { get; set;} = availableStaff;

        public void MarketInterface(Action action) {
            Console.WriteLine("Welcome to the Market!");
            Console.WriteLine("1. Buy Player [Not Implemented]");
            Console.WriteLine("2. Buy Staff [Not Implemented]");
            Console.WriteLine("3. Sell Player [Not Implemented]");
            Console.WriteLine("4. Sell Staff [Not Implemented]");
            Console.WriteLine("5. Exit Market");
            Console.WriteLine();
            string input = InputReader.ReadText("Enter your choice: ");
            while(true)
            {
                switch(input)
                {
                    /*
                    case "1":
                        BuyPlayer();
                        break;
                    case "2":
                        BuyStaff();
                        break;
                    case "3":
                        SellPlayer();
                        break;
                    case "4":
                        SellStaff();
                        break;
                    */
                    case "5":
                        Console.WriteLine("Exiting Market...");
                        ExitMarket(action);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void SellStaff()
        {
            throw new NotImplementedException();
        }

        private void SellPlayer()
        {
            throw new NotImplementedException();
        }

        private void BuyStaff()
        {
            throw new NotImplementedException();
        }

        private void BuyPlayer()
        {
            throw new NotImplementedException();
        }

        private void ExitMarket(Action returnToMenu)
        {
            returnToMenu();
        }
    }
}