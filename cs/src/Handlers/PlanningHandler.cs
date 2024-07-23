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
            while (true)
            {
                Console.WriteLine($"\n\nRound {gameHandler.Round}");
                Console.WriteLine($"Opponent: {gameHandler.OpponentTeam.Name}");
                Console.WriteLine($"Potential Score: {gameHandler.CalculateScore(true)[1]} (+/-)");
                Console.WriteLine("1. Choose Lineup and Start");
                Console.WriteLine("2. Medical Attention");

                switch (InputReader.ReadText("Choose Option: "))
                {
                    case "1":
                        Lineup = ChooseLineup();
                        return;

                    case "2":
                        MedicalAttention();
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        private void MedicalAttention()
        {
            List<Person> injuredPlayers = [];
            while (true)
            {
                foreach (Person player in gameHandler.PlayerTeam.Players)
                {
                    if (player.Status == "Injured")
                    {
                        injuredPlayers.Add(player);
                        Console.Write(injuredPlayers.Count + ". ");
                        player.PrintInfo();
                        Console.WriteLine($"Cost To Give Aid: {player.Cost / 2}");
                    }
                }
                
                if (injuredPlayers.Count == 0)
                {
                    Console.WriteLine("No injured players.");
                    return;
                }

                int choice = Convert.ToInt32(InputReader.ReadText("Choose player to give Medical Atention: "));

                if (choice < 0 || choice > injuredPlayers.Count)
                {
                    Console.WriteLine("Invalid choice. Try again.");
                }
                else if (choice == 0)
                {
                    return;
                }
                else
                {
                    Person p = injuredPlayers[choice - 1];
                    if (gameHandler.PlayerTeam.Budget < p.Cost / 2)
                    {
                        Console.WriteLine("Not enough budget to give medical attention.");
                        return;
                    }
                    else
                    {
                        p.Status = "Active";
                        p.Cost *= 2;
                        if (p.Value < 0)
                        {
                            p.Value *= -2;
                        }
                        else
                        {
                            p.Value *= 2;
                        }
                        Console.WriteLine($"{p.Name} is now active.");
                        return;
                    }
                }
            }
        }

        public Stack<Person> ChooseLineup()
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