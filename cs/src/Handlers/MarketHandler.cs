using sports_game.src.Models;
using sports_game.src.Services;

namespace sports_game.src.Handlers
{
    public class MarketHandler(GameHandler gameHandler)
    {
        private int HeldRound { get; set; }
        private List<Person> BenchPlayers { get; set;}
        private List<Person> BenchStaff { get; set;}
        private List<Person> PurchaseablePlayers { get; set; } = [];
        private List<Person> PurchaseableStaff { get; set; } = [];

        public void MarketInterface() {

            if (HeldRound != gameHandler.Round)
            {
                GenerateMarketLists();
                HeldRound = gameHandler.Round;
            }

            while(true)
            {
                Console.WriteLine("\n\nWelcome to the Market!");
                Console.WriteLine($"Budget: {gameHandler.PlayerTeam.Budget}");
                Console.WriteLine("1. Buy Player");
                Console.WriteLine("2. Buy Staff");
                Console.WriteLine("3. Sell Player");
                Console.WriteLine("4. Sell Staff");
                Console.WriteLine("0. Exit Market\n");

                string input = InputReader.ReadText("Enter your choice: ");
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
                    case "0":
                        Console.WriteLine("Exiting Market...");
                        ExitMarket();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void GenerateMarketLists()
        {
            BenchPlayers = gameHandler.PlayerTeam.BenchedPlayers;
            BenchStaff = gameHandler.PlayerTeam.BenchedStaff;
            PurchaseablePlayers.Clear();
            PurchaseableStaff.Clear();

            while (PurchaseablePlayers.Count != 4)
            {
                Person newPlayer = gameHandler.PlayerCategoryService.PickItem();
                if (!PurchaseablePlayers.Contains(newPlayer))
                {
                    PurchaseablePlayers.Add(newPlayer);
                }
            }
            while (PurchaseableStaff.Count != 4)
            {
                Person newStaff = gameHandler.StaffCategoryService.PickItem();
                if (!PurchaseableStaff.Contains(newStaff))
                {
                    PurchaseableStaff.Add(newStaff);
                }
            }
                
        }

        private void SellStaff()
        {
            if (BenchStaff.Count != 0)
            {
                Console.WriteLine();
                for(int i = 0; i < BenchStaff.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {BenchStaff[i].Name} | {BenchStaff[i].CurrentPosition.Name} | {BenchStaff[i].Cost}");
                }
                Console.WriteLine("0. Exit\n");

                string input = InputReader.ReadText("Enter the number of the staff you want to sell: ");

                if (int.TryParse(input, out int index) && index > 0 && index <= BenchStaff.Count)
                {
                    Person chosenStaff = BenchStaff[index - 1];
                    gameHandler.AddAvailablePerson(chosenStaff);
                    gameHandler.PlayerTeam.Budget += chosenStaff.Cost;
                    BenchStaff.RemoveAt(index - 1);
                    
                    Console.WriteLine("Staff sold successfully!");
                }
                else if (input == "0")
                {
                    MarketInterface();

                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("You have no staff to sell.");
                MarketInterface();
            }

        }

        private void SellPlayer()
        {
            if (BenchPlayers.Count != 0)
            {
                Console.WriteLine();
                for(int i = 0; i < BenchPlayers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {BenchPlayers[i].Name} | {BenchPlayers[i].CurrentPosition.Name} | {BenchPlayers[i].Cost}");
                }
                Console.WriteLine("0. Exit\n");

                string input = InputReader.ReadText("Enter the number of the player you want to sell: ");

                if (int.TryParse(input, out int index) && index > 0 && index <= BenchPlayers.Count)
                {
                    Person chosenPlayer = BenchPlayers[index - 1];
                    gameHandler.AddAvailablePerson(chosenPlayer);
                    gameHandler.PlayerTeam.Budget += chosenPlayer.Cost;
                    BenchPlayers.RemoveAt(index - 1);
                    
                    Console.WriteLine("Player sold successfully!");
                }
                else if (input == "0")
                {
                    MarketInterface();

                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("You have no players to sell.");
                MarketInterface();
            }
        }

        private void BuyStaff()
        {
            if (PurchaseableStaff.Count > 0)
            {
                Console.WriteLine();
                for(int i = 0; i < PurchaseableStaff.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {PurchaseableStaff[i].Name} | {PurchaseableStaff[i].Value} | {PurchaseableStaff[i].CurrentPosition.Modifier} | {PurchaseableStaff[i].Cost} | {PurchaseableStaff[i].Rarity}");
                }
                Console.WriteLine("0. Exit\n");

                string input = InputReader.ReadText("Enter the number of the staff you want to buy: ");

                if (int.TryParse(input, out int index) && index > 0 && index <= PurchaseableStaff.Count)
                {
                    Person chosenStaff = PurchaseableStaff[index - 1];
                    if (gameHandler.PlayerTeam.Budget >= chosenStaff.Cost)
                    {
                        BenchStaff.Add(chosenStaff);
                        gameHandler.PlayerTeam.Budget -= chosenStaff.Cost;
                        PurchaseableStaff.RemoveAt(index - 1);
                        
                        Console.WriteLine("Staff bought successfully!");
                    }
                    else
                    {
                        Console.WriteLine("You do not have enough budget to buy this staff.");
                    }
                }
                else if (input == "0")
                {
                    MarketInterface();

                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("There are no staff available to buy.");
                MarketInterface();
            }
        }

        private void BuyPlayer()
        {
            if (PurchaseablePlayers.Count > 0)
            {
                Console.WriteLine();
                for(int i = 0; i < PurchaseablePlayers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {PurchaseablePlayers[i].Name} | {PurchaseablePlayers[i].Value} | {PurchaseablePlayers[i].CurrentPosition.Modifier} | {PurchaseablePlayers[i].Cost} | {PurchaseablePlayers[i].Rarity}");
                }
                Console.WriteLine("0. Exit\n");

                string input = InputReader.ReadText("Enter the number of the player you want to buy: ");

                if (int.TryParse(input, out int index) && index > 0 && index <= PurchaseablePlayers.Count)
                {
                    Person chosenPlayer = PurchaseablePlayers[index - 1];
                    if (gameHandler.PlayerTeam.Budget >= chosenPlayer.Cost)
                    {
                        BenchPlayers.Add(chosenPlayer);
                        gameHandler.PlayerTeam.Budget -= chosenPlayer.Cost;
                        PurchaseablePlayers.RemoveAt(index - 1);
                        
                        Console.WriteLine("Player bought successfully!");
                    }
                    else
                    {
                        Console.WriteLine("You do not have enough budget to buy this player.");
                    }
                }
                else if (input == "0")
                {
                    MarketInterface();

                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("There are no players available to buy.");
                MarketInterface();
            }
        }

        private void ExitMarket()
        {
            gameHandler.PlayGame();
        }
    }
}