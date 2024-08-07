using sports_game.src.Handlers;
using sports_game.src.Models;
using sports_game.src.Services;

namespace sports_game.src.Entities
{    
    public class Team(GameHandler gh, string name, string icon = "_", bool isPlayer = false)
    {
        public string Name { get; set; } = name;
        public List<Person> Staff { get; set; } = [];
        public List<Person> Players { get; set; } = [];
        public List<Person> BenchedPlayers { get; set; } = [];
        public List<Person> BenchedStaff { get; set; } = [];
        public string Icon { get; set; } = icon;
        public int Score { get; set; }
        public int Budget { get; set; }
        static public int Interest { get; set; } = 10;
        public bool IsPlayer { get; set; } = isPlayer;
        public EffectHandler EffectHandlerTeam { get; set; }
        public GameHandler GameHandlerLocal { get; set; } = gh;

        public void TeamDataInitialize()
        {
            EffectHandlerTeam = new EffectHandler(this);
        }

        public void ReplacePlayer(Person p, Person replacement)
        {
            RemovePerson(p);
            RemovePerson(replacement);
            AddPerson(p, true);
            AddPerson(replacement, false);
        }

        public void RemovePerson(Person p)
        {
            if (!Players.Remove(p) && !BenchedPlayers.Remove(p) && !Staff.Remove(p) && !BenchedStaff.Remove(p))
            {
                throw new Exception("Person not found");
            }

            EffectHandlerTeam.RemoveEffects(p);
        }

        public void AddPerson(Person p, bool isBenched)
        {
            switch (isBenched)
            {
                case true:
                    if (p.CurrentPositionID.Contains("PP"))
                    {
                        BenchedPlayers.Add(p);
                    }
                    else
                    {
                        BenchedStaff.Add(p);
                    }
                    break;

                case false:
                    if (p.CurrentPositionID.Contains("PP"))
                    {
                        Players.Add(p);
                    }
                    else
                    {
                        Staff.Add(p);
                    }
                    break;
            }
        }

        public void CalcInterest()
        {
            int tempBudget = Budget;
            if (IsPlayer)
            {
                Console.Write("Interest: ");
            }
            while (tempBudget != 0)
            {
                if (tempBudget >= 100)
                {
                    Budget += Interest;
                    tempBudget -= 100;
                }
                else
                {
                    break;
                }
                Console.Write("+ ");
            }
            if (IsPlayer)
            {
                Console.WriteLine($"\nBudget: {Budget}");
            }

        }
    }
}