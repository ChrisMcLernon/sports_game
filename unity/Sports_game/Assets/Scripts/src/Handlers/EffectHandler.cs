using sports_game.src.Entities;
using sports_game.src.Models;

namespace sports_game.src.Handlers
{
    public class EffectHandler
    {
        public Dictionary<string, List<Effect>> Effects { get; set; } = [];

        public void RemoveEffects(Person person)
        {
            Effects.Remove(person.Name);
        }

        public void AddEffects(Person person)
        {
            if (!Effects.TryGetValue(person.Name, out List<Effect>? value))
            {
                Effects[person.Name] = [];
                foreach (var effect in person.Effects)
                {
                    Effects[person.Name].Add(effect);
                }
            }
            else if (Effects.TryGetValue(person.Name, out List<Effect>? key))
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
            foreach (var effect in Effects[person.Name])
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