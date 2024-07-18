using sports_game.src.Models;
using sports_game.src.Services;

namespace sports_game.src.Handlers
{
    public class MarketHandler(List<Person> availablePlayers, List<Person> availableStaff)
    {
        private List<Person> AvailablePlayers { get; set;} = availablePlayers;
        private List<Person> AvailableStaff { get; set;} = availableStaff;

        public void MarketMenu() {
            Console.WriteLine("Welcome to the Market!");
            Console.WriteLine("1. Buy Player");
            Console.WriteLine("2. Buy Staff");
            Console.WriteLine("3. Sell Player");
            Console.WriteLine("4. Sell Staff");
            Console.WriteLine("5. Exit Market");
            Console.WriteLine();
            string input = InputReader.ReadText("Enter your choice: ");
            while(true)
            {
                switch(input)
                {
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
                    case "5":
                        Console.WriteLine("Exiting Market...");
                        ExitMarket();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void ExitMarket()
        {
            throw new NotImplementedException();
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
    }
}