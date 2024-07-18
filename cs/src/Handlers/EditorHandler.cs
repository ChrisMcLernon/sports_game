using sports_game.src.Services;

namespace sports_game.src.Handlers
{
    public class EditorHandler()
    {
        public void EditorInterface(Action action) {
            Console.WriteLine("Welcome to the Editor!");
            Console.WriteLine("1. Edit Players [Not Implemented]");
            Console.WriteLine("2. Edit Staff [Not Implemented]");
            Console.WriteLine("3. Edit Teams [Not Implemented]");
            Console.WriteLine("4. Edit Positions [Not Implemented]");
            Console.WriteLine("5. Exit Editor [Not Implemented]");
            Console.WriteLine();
            string input = InputReader.ReadText("Enter your choice: ");
            while(true)
            {
                switch(input)
                {
                    /*
                    case "1":
                        EditPlayers();
                        break;
                    case "2":
                        EditStaff();
                        break;
                    case "3":
                        EditTeams();
                        break;
                    case "4":
                        EditPositions();
                        break;
                    */
                    case "5":
                        Console.WriteLine("Exiting Editor...");
                        ExitEditor(action);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void EditPlayers()
        {
            throw new NotImplementedException();
        }

        private void EditStaff()
        {
            throw new NotImplementedException();
        }

        private void EditTeams()
        {
            throw new NotImplementedException();
        }

        private void EditPositions()
        {
            throw new NotImplementedException();
        }

        private void ExitEditor(Action returnToMenu)
        {
            returnToMenu();
        }
    }
}
