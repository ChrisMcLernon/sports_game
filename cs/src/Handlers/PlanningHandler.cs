using sports_game.src.Models;
using sports_game.src.Services;

namespace sports_game.src.Handlers
{
    public class PlanningHandler(GameHandler gh)
    {
        private GameHandler gameHandler = gh;
        public Stack<Person> Lineup { get; set; }

        public void PlanningInterface()
        {
            Console.WriteLine($"\n\nRound {gameHandler.Round}");
            if (gameHandler.Round % 3 == 0)
            {
                Console.WriteLine($"Boss Round! (x{Math.Round(gameHandler.Round * 0.5, 1)} Points)");
            }
            else
            {
                Console.WriteLine($"(x{Math.Round(gameHandler.Round * 0.2, 1)} Points)");
            }
            Console.WriteLine($"Opponent: {gameHandler.OpponentTeam.Name}");
            Console.WriteLine($"Choose your order:");
            Lineup = ChooseLineup();
        }

        private Stack<Person> ChooseLineup()
        {
            List<Person> lineupList = gameHandler.PlayerTeam.Players;
            lineupList.AddRange(gameHandler.PlayerTeam.Staff);
            Stack<Person> lineup = new();

            while (lineupList.Count != 0)
            {
                for (int i = 0; i < lineupList.Count; i++)
                {
                    Person player = gameHandler.PlayerTeam.Players[i];
                    Console.Write(i + 1 + ". ");
                    player.PrintInfo();
                }

                int choice = Convert.ToInt32(InputReader.ReadText("Choose player: "));
                if (choice < 1 || choice > lineupList.Count)
                {
                    Console.WriteLine("Invalid choice. Try again.");
                }
                else
                {
                    Person p = lineupList[choice - 1];
                    lineup.Push(p);
                    lineupList.Remove(p);
                }
            }
            return lineup;
        }
    }
}