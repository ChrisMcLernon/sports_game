using System.Security;
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
            Effects.Add(person.CurrentPosition.Name, person.Effects);
        }

        public float ApplyPersonEffects(Person person)
        {
            float totalEffect = 0;
            foreach (var effect in Effects[person.CurrentPosition.Name])
            {
                Console.WriteLine($"{person.Name} - {person.CurrentPosition.Name}");
                Console.WriteLine($"Effect: {effect.Description}");
                switch (effect.Description)
                {
                    case "Increase Value":
                        totalEffect += effect.Value;
                        Console.WriteLine($"Total Effect: {totalEffect}");
                        break;
                    
                    case "Decrease Value":
                        totalEffect -= effect.Value;
                        Console.WriteLine($"Total Effect: {totalEffect}");
                        break;
                    
                    case "Increase Cost":
                        Console.WriteLine($"Old Cost: {person.Cost}");
                        person.Cost += effect.Value;
                        Console.WriteLine($"New Cost: {person.Cost}");
                        break;

                    case "Decrease Cost":
                        Console.WriteLine($"Old Cost: {person.Cost}");
                        person.Cost -= effect.Value;
                        Console.WriteLine($"New Cost: {person.Cost}");
                        break;
                }
            }
            Console.WriteLine($"Total Effect: {totalEffect}");
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
                            Console.WriteLine($"Old Budget: {Team.Budget}");
                            Team.Budget += e.Value;
                            Console.WriteLine($"New Budget: {Team.Budget}");
                            break;
                        
                        case "Decrease Budget":
                            Console.WriteLine($"Old Budget: {Team.Budget}");
                            Team.Budget -= e.Value;
                            Console.WriteLine($"New Budget: {Team.Budget}");
                            break;
                        
                        case "Increase Interest":
                            Console.WriteLine($"Old Interest: {Team.Interest}");
                            Team.Interest += e.Value;
                            Console.WriteLine($"New Interest: {Team.Interest}");
                            break;
                        
                        case "Decrease Interest":
                            Console.WriteLine($"Old Interest: {Team.Interest}");
                            Team.Interest -= e.Value;
                            Console.WriteLine($"New Interest: {Team.Interest}");
                            break;
                    }
                }
            }
        }
    }
}