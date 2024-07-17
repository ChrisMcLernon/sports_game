using sports_game.src.Handlers;
using sports_game.src.Models;
using sports_game.src.Services;

namespace sports_game.src.Entities
{    
    public class Team(string name, string icon = "_", string sport = "FOOTBALL", int score = 0, int budget = 0,int interest = 10, bool isPlayer = false)
    {
        public string Name { get; set; } = name;
        public List<Person> Staff { get; set; } = [];
        public List<Person> Players { get; set; } = [];
        public string Icon { get; set; } = icon;
        public string Sport { get; set; } = sport;
        public int Score { get; set; } = score;
        public int Budget { get; set; } = budget;
        public int Interest { get; set; } = interest;
        public bool IsPlayer { get; set; } = isPlayer;
        public EffectHandler EffectHandlerTeam { get; set; } = new EffectHandler();
        public List<string> PossiblePlayerPositions { get; set; } = [];
        public List<string> PossibleStaffPositions { get; set; } = [];

        public void GeneratePossiblePositions()
        {
            if (Sport == "FOOTBALL"){
                PossiblePlayerPositions = JsonReader.Read<List<string>>("Football_Player_Positions");
                PossibleStaffPositions = JsonReader.Read<List<string>>("Football_Staff_Positions");
            }
        }

        public void AddPerson(Person p)
        {
            if (PossiblePlayerPositions.Contains(p.CurrentPosition.Name) && !PositionFilled(p.CurrentPosition))
            {
                Players.Add(p);
                EffectHandlerTeam.AddEffects(p);

                Console.WriteLine($"Player: {p.Name}, Age: {p.Age}, Value: {p.Value}, " +
                              $"Position: {p.CurrentPosition.Name}, Modifier: {p.CurrentPosition.Modifier}, " +
                              $"Size: {p.CurrentPosition.Size}, Cost: {p.Cost}, Status: {p.Status}");

                Console.WriteLine("Effects:");
                foreach (var effect in EffectHandlerTeam.Effects[p.CurrentPosition.Name])
                {
                    Console.WriteLine($"- {effect.Name}: {effect.Description} (+{effect.Value})");
                }

            }
            else if (PossibleStaffPositions.Contains(p.CurrentPosition.Name) && !PositionFilled(p.CurrentPosition))
            {
                Staff.Add(p);
                EffectHandlerTeam.AddEffects(p);
            }
        }

        public void CalcInterest()
        {
            Budget += Interest * (Budget % 20);
        }

        public void ReplacePlayer(Person p, Person replacement)
        {
            if (!Players.Remove(p))
                Staff.Remove(p);
            AddPerson(replacement);
        }

        public bool PositionFilled(Position pos)
        {
            int numPlayersInPosition = 0;
            foreach (var player in Players)
            {
                if (player.CurrentPosition.Name == pos.Name)
                {
                    numPlayersInPosition++;
                    if (numPlayersInPosition == pos.Size)
                    {
                        Console.WriteLine(pos.Name);
                        Console.WriteLine("Position is already filled");
                        return true;
                    }
                }
            }
            return false;
        }
    }
}