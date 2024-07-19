using sports_game.src.Models;
using sports_game.src.Services;

namespace sports_game.src.Handlers
{
    public class EditorHandler(GameHandler gameHandler)
    {
        private List<Person> BenchPlayers { get; set;}
        private List<Person> BenchStaff { get; set;}
        
        public void EditorInterface() 
        {
            InitializeData();

            Console.WriteLine("Welcome to the Editor!");
            Console.WriteLine("1. Edit Players");
            Console.WriteLine("2. Edit Staff [Not Implemented]");
            Console.WriteLine("0. Exit Editor");
            Console.WriteLine();
            while(true)
            {
                string input = InputReader.ReadText("Enter your choice: ");
                switch(input)
                {
                    case "1":
                        EditPlayers();
                        break;
                    /*
                    case "2":
                        EditStaff();
                        break;
                    */
                    case "0":
                        Console.WriteLine("Exiting Editor...");
                        ExitEditor();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void InitializeData()
        {
            BenchPlayers = gameHandler.PlayerTeam.BenchedPlayers;
            BenchStaff = gameHandler.PlayerTeam.BenchedStaff;
        }

        private void EditPlayers()
        {
            if (BenchPlayers.Count == 0)
            {
                Console.WriteLine("No players to edit.");
                return;
            }
            else
            {
                Person replacement;
                Person replacee;

                Console.WriteLine("Benched Players:");
                for (int i = 0; i < BenchPlayers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {BenchPlayers[i].Name}");
                }

                string input = InputReader.ReadText("Enter the number of the player you choose: ");

                if (int.TryParse(input, out int index) && index > 0 && index <= BenchPlayers.Count)
                {
                    replacement = BenchPlayers[index - 1];
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                    return;
                }

                Console.WriteLine("Players:");
                for (int i = 0; i < gameHandler.PlayerTeam.Players.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {gameHandler.PlayerTeam.Players[i].Name}");
                }

                input = InputReader.ReadText("Enter the number of the player you want to replace: ");

                if (int.TryParse(input, out index) && index > 0 && index <= gameHandler.PlayerTeam.Players.Count)
                {
                    replacee = gameHandler.PlayerTeam.Players[index - 1];
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                    return;
                }

                gameHandler.PlayerTeam.ReplacePlayer(replacee, replacement);

                if (gameHandler.PlayerTeam.Players.Contains(replacement) && !gameHandler.PlayerTeam.BenchedPlayers.Contains(replacee))
                {
                    gameHandler.PlayerTeam.BenchedPlayers.Add(replacee);
                    gameHandler.PlayerTeam.BenchedPlayers.Remove(replacement);
                    Console.WriteLine("Player replaced successfully.");
                }
                else
                {
                    Console.WriteLine("Player not replaced.");
                }


                
            
            }
        }

        private void EditStaff()
        {
            throw new NotImplementedException();
        }

        private void ExitEditor()
        {
            gameHandler.PlayGame();
        }
    }
}
