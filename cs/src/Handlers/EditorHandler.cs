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

            while(true)
            {
                Console.WriteLine("\n\nWelcome to the Editor!");
                Console.WriteLine("1. Edit Players");
                Console.WriteLine("2. Edit Staff");
                Console.WriteLine("0. Exit Editor");

                string input = InputReader.ReadText("Enter your choice: ");
                switch(input)
                {
                    case "1":
                        EditPlayers();
                        break;
                    case "2":
                        EditStaff();
                        break;
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
                while (true)
                {
                    Person replacement = null;
                    Person replacee = null;

                    Console.WriteLine("\nPlayers:");
                    for (int i = 0; i < gameHandler.PlayerTeam.Players.Count; i++)
                    {
                        gameHandler.PlayerTeam.Players[i].PrintInfo();
                    }

                    Console.WriteLine("\nBenched Players:");
                    for (int i = 0; i < BenchPlayers.Count; i++)
                    {
                        Console.Write($"{i + 1}. ");
                        BenchPlayers[i].PrintInfo();
                    }
                    Console.WriteLine("0. Cancel");

                    string input = InputReader.ReadText("Enter the number of the player you choose: ");

                    if (int.TryParse(input, out int index) && index > 0 && index <= BenchPlayers.Count)
                    {
                        replacement = BenchPlayers[index - 1];
                    }
                    else if (input == "0")
                    {
                        EditorInterface();
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice.");
                    }
                    if (replacement != null)
                    {
                        Console.WriteLine("\nPlayers:");
                        for (int i = 0; i < gameHandler.PlayerTeam.Players.Count; i++)
                        {
                            Console.Write($"{i + 1}. ");
                            gameHandler.PlayerTeam.Players[i].PrintInfo();
                        }
                        Console.WriteLine("0. Cancel");

                        input = InputReader.ReadText("Enter the number of the player you want to replace: ");

                        if (int.TryParse(input, out index) && index > 0 && index <= gameHandler.PlayerTeam.Players.Count)
                        {
                            replacee = gameHandler.PlayerTeam.Players[index - 1];
                        }
                        else if (input == "0")
                        {
                            EditorInterface();
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice.");
                            return;
                        }
                        if (replacement == null || replacee == null)
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
            }
        }

        private void EditStaff()
        {
            if (BenchStaff.Count == 0)
            {
                Console.WriteLine("No staff to edit.");
                return;
            }
            else
            {
                while (true)
                {
                    Person replacement = null;
                    Person replacee = null;

                    Console.WriteLine("\nStaff:");
                    for (int i = 0; i < gameHandler.PlayerTeam.Staff.Count; i++)
                    {
                        gameHandler.PlayerTeam.Staff[i].PrintInfo();
                    }
                    Console.WriteLine("Benched Staff:");
                    for (int i = 0; i < BenchStaff.Count; i++)
                    {
                        Console.Write($"{i + 1}. ");
                        BenchStaff[i].PrintInfo();
                    }
                    Console.WriteLine("0. Cancel");

                    string input = InputReader.ReadText("Enter the number of the staff you choose: ");

                    if (int.TryParse(input, out int index) && index > 0 && index <= BenchStaff.Count)
                    {
                        replacement = BenchStaff[index - 1];
                    }
                    else if (input == "0")
                    {
                        EditorInterface();
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice.");
                        return;
                    }

                    Console.WriteLine("Staff:");
                    for (int i = 0; i < gameHandler.PlayerTeam.Staff.Count; i++)
                    {
                        Console.Write($"{i + 1}. ");
                        gameHandler.PlayerTeam.Staff[i].PrintInfo();
                    }
                    Console.WriteLine("0. Cancel");

                    input = InputReader.ReadText("Enter the number of the staff you want to replace: ");

                    if (int.TryParse(input, out index) && index > 0 && index <= gameHandler.PlayerTeam.Staff.Count)
                    {
                        replacee = gameHandler.PlayerTeam.Staff[index - 1];
                    }
                    else if (input == "0")
                    {
                        EditorInterface();
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice.");
                        return;
                    }
                    if (replacement == null || replacee == null)
                    {
                        Console.WriteLine("Invalid choice.");
                        return;
                    }
                    gameHandler.PlayerTeam.ReplacePlayer(replacee, replacement);

                    if (gameHandler.PlayerTeam.Staff.Contains(replacement) && !gameHandler.PlayerTeam.BenchedStaff.Contains(replacee))
                    {
                        Console.WriteLine("Staff replaced successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Staff not replaced.");
                    }
                }
            }
        }

        private void ExitEditor()
        {
            gameHandler.PlayGame();
        }
    }
}
