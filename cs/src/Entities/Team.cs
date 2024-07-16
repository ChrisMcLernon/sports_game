using sports_game.src.Handlers;
using sports_game.src.Models;

namespace sports_game.src.Entities
{    
    public class Team(string name, List<Person> staff, List<Person> players, Person leader, string icon = "_",
                        string sport = "FOOTBALL", int score = 0, int budget = 0,int interest = 10, bool isPlayer = false)
    {
        public string Name { get; set; } = name;
        public List<Person> Staff { get; set; } = staff;
        public List<Person> Players { get; set; } = players;
        public Person Leader { get; set; } = leader;
        public string Icon { get; set; } = icon;
        public string Sport { get; set; } = sport;
        public int Score { get; set; } = score;
        public int Budget { get; set; } = budget;
        public int Interest { get; set; } = interest;
        public bool IsPlayer { get; set; } = isPlayer;
        public EffectHandler EffectHandlerTeam { get; set; } = new EffectHandler();

        public void CalcInterest()
        {
            Budget += Interest * (Budget % 20);
        }

        public void ReplacePlayer(Person player, Person replacement)
        {
            if (PositionFilled(player.CurrentPosition))
            {
                Players.Remove(player);
                foreach (var effect in player.Effects)
                {
                    EffectHandlerTeam.RemoveEffect(effect);
                }
            }
            Players.Add(replacement);
            foreach (var effect in replacement.Effects)
            {
                EffectHandlerTeam.AddEffect(effect);
            }
        }

        public void ReplaceStaff(Person staff, Person replacement)
        {
            if (PositionFilled(staff.CurrentPosition)){
                Staff.Remove(staff);
                foreach (var effect in staff.Effects)
                {
                    EffectHandlerTeam.RemoveEffect(effect);
                }
            }
            Staff.Add(replacement);
            foreach (var effect in replacement.Effects){
                EffectHandlerTeam.AddEffect(effect);
            }
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
            return false;
        }
    }
}