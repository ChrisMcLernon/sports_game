using sports_game.src.Handlers;
using sports_game.src.Models;
using sports_game.src.Services;

namespace sports_game.src.Entities
{    
    public class Team(string name, string icon = "_", string sport = "FOOTBALL", bool isPlayer = false)
    {
        public string Name { get; set; } = name;
        public List<Person> Staff { get; set; } = [];
        public List<Person> Players { get; set; } = [];
        public List<Person> BenchedPlayers { get; set; } = [];
        public List<Person> BenchedStaff { get; set; } = [];
        public string Icon { get; set; } = icon;
        public string Sport { get; set; } = sport;
        public int Score { get; set; }
        static public int Budget { get; set; }
        static public int Interest { get; set; }
        public bool IsPlayer { get; set; } = isPlayer;
        public EffectHandler EffectHandlerTeam { get; set; } = new EffectHandler();
        public List<string> PossiblePlayerPositions { get; set; } = [];
        public List<string> PossibleStaffPositions { get; set; } = [];

        public void ReplacePlayer(Person p, Person replacement)
        {
            RemovePerson(p);
            AddPerson(replacement);
        }

        public void RemovePerson(Person p)
        {
            if (!Players.Remove(p))
                Staff.Remove(p);
            EffectHandlerTeam.RemoveEffects(p);
        }

        public void AddPerson(Person p)
        {
            if (PossiblePlayerPositions.Contains(p.CurrentPosition.Name) && !PositionFilled(p.CurrentPosition))
            {
                Players.Add(p);
                EffectHandlerTeam.AddEffects(p);
            }
            else if (PossibleStaffPositions.Contains(p.CurrentPosition.Name) && !PositionFilled(p.CurrentPosition))
            {
                Staff.Add(p);
                EffectHandlerTeam.AddEffects(p);
            }
        }

        public void GeneratePossiblePositions()
        {
            if (Sport == "FOOTBALL"){
                PossiblePlayerPositions = JsonReader.Read<List<string>>("Football_Player_Positions");
                PossibleStaffPositions = JsonReader.Read<List<string>>("Football_Staff_Positions");
            }
        }

        public void CalcInterest()
        {
            Budget += Interest * (Budget % 20);
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
                        return true;
                    }
                }
            }
            foreach (var members in Staff)
            {
                if (members.CurrentPosition.Name == pos.Name)
                {
                    numPlayersInPosition++;
                    if (numPlayersInPosition == pos.Size)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}