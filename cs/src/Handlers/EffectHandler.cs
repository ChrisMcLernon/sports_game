using sports_game.src.Entities;
using sports_game.src.Models;

namespace sports_game.src.Handlers
{
    public class EffectHandler
    {
        static public Dictionary<string, List<Effect>> Effects { get; set; } = [];

        public void RemoveEffects(Person person)
        {
            Effects.Remove(person.CurrentPosition.Name);
        }

        public void AddEffects(Person person)
        {
            Effects.Add(person.CurrentPosition.Name, person.Effects);
        }

        public float ApplyPersonEffects(Person person)
        {
            float totalEffect = 0;
            foreach (var effect in Effects[person.CurrentPosition.Name])
            {
                switch (effect.Description)
                {
                    case "Increase Value":
                        totalEffect += effect.Value;

                        break;
                    
                    case "Decrease Value":
                        totalEffect -= effect.Value;
                        break;
                    
                    case "Increase Cost":
                        person.Cost += effect.Value;
                        break;

                    case "Decrease Cost":
                        person.Cost -= effect.Value;
                        break;
                }
            }
            return totalEffect;
        }

        public void ApplyTeamEffects()
        {
            foreach (var effect in Effects)
            {
                foreach (var e in effect.Value)
                {
                    switch (e.Description)
                    {
                        case "Increase Budget":
                            Team.Budget += e.Value;
                            break;
                        
                        case "Decrease Budget":
                            Team.Budget -= e.Value;
                            break;
                        
                        case "Increase Interest":
                            Team.Interest += e.Value;
                            break;
                        
                        case "Decrease Interest":
                            Team.Interest -= e.Value;
                            break;
                    }
                }
            }
        }
    }
}