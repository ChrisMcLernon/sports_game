using sports_game.src.Entities;
using sports_game.src.Models;

namespace sports_game.src.Handlers
{
    public class EffectHandler
    {
        public Dictionary<string, List<Effect>> Effects { get; set; } = [];

        public void RemoveEffects(Person person)
        {
            Effects.Remove(person.ID);
        }

        public void AddEffects(Person person)
        {
            if (!Effects.TryGetValue(person.ID, out List<Effect>? value))
            {
                Effects[person.ID] = [];
                foreach (var effect in person.Effects)
                {
                    Effects[person.ID].Add(effect);
                }
            }
        }

        public int ApplyPersonEffects(Person person)
        {
            int totalEffect = person.Value;
            foreach (var effect in Effects[person.ID])
            {
                switch (effect.Description)
                {
                    case "Increase Value":
                        totalEffect += effect.Value;
                        break;
                    
                    case "Decrease Value":
                        person.Value -= effect.Value;
                        break;

                    case "Increase Cost":
                        person.Cost += effect.Value;
                        break;
                    
                    case "Decrease Cost":
                        person.Cost -= effect.Value;
                        break;
                }
            }

            foreach (var effect in Effects)
            {
                foreach (var e in effect.Value)
                {
                    switch (e.Name)
                    {
                        case "Multiply Position":
                            if (e.Target == person.CurrentPositionID)
                            {
                                totalEffect *= e.Value;
                            }
                            break;
                    }
                }
            }
            return totalEffect;
        }
    }
}