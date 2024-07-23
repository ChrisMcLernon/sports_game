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
            Console.WriteLine($"Opponent: {gameHandler.OpponentTeam.Name}");
            Console.WriteLine($"Potential Score: {gameHandler.CalculateScore(true)[1]} (+/-)");
            Console.WriteLine($"Choose your order:");
            Lineup = ChooseLineup();
        }

        private Stack<Person> ChooseLineup()
        {

            List<Person> lineupList = [.. gameHandler.PlayerTeam.Players];
            lineupList.AddRange(gameHandler.PlayerTeam.Staff);
            Stack<Person> lineup = new();

            while (lineupList.Count != 0)
            {
                for (int i = 0; i < lineupList.Count; i++)
                {
                    Person player = lineupList[i];
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