using sports_game.src.Entities;
using sports_game.src.Models;

namespace sports_game.src.Handlers
{
    public class EffectHandler
    {
        public Dictionary<string, List<Effect>> Effects { get; set; } = [];

        public void RemoveEffects(Person person)
        {
            Effects.Remove(person.CurrentPosition.Name);
        }

        public void AddEffects(Person person)
        {
            if (!Effects.TryGetValue(person.CurrentPosition.Name, out List<Effect>? value))
            {
                Effects[person.CurrentPosition.Name] = [];
                foreach (var effect in person.Effects)
                {
                    Effects[person.CurrentPosition.Name].Add(effect);
                }
            }
            else if (Effects.TryGetValue(person.CurrentPosition.Name, out List<Effect>? key))
            {
                foreach (var effect in person.Effects)
                {
                    key.Add(effect);
                }
            }
        }

        public int ApplyPersonEffects(Person person)
        {
            int totalEffect = 0;
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