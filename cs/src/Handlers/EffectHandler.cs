using sports_game.src.Entities;
using sports_game.src.Models;

namespace sports_game.src.Handlers
{
    public class EffectHandler(Team team)
    {
        public Dictionary<string, List<Effect>> Effects { get; set; } = [];
        public Team TeamLocal { get; set; } = team;

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
                switch (effect.Name)
                {
                    case "Increase Value":
                        person.Value += effect.Value;
                        if (TeamLocal.IsPlayer)
                        {
                            Console.WriteLine($"Added {effect.Value} to Value of {person.Name}");

                        }
                        break;
                    
                    case "Decrease Value":
                        if (person.Value < 0)
                        {
                            person.Value = 0;
                        }
                        else 
                        {
                            person.Value -= effect.Value;
                        }
                        if (TeamLocal.IsPlayer)
                        {
                            Console.WriteLine($"Subtracted {effect.Value} from Value of {person.Name}");
                        }
                        break;

                    case "Increase Cost":
                        person.Cost += effect.Value;
                        if (TeamLocal.IsPlayer)
                        {
                            Console.WriteLine($"Added {effect.Value} to Cost of {person.Name}");
                        }
                        break;
                    
                    case "Decrease Cost":
                        if (person.Cost < 0)
                        {
                            person.Cost = 0;
                        }
                        else
                        {
                            person.Cost -= effect.Value;
                        }
                        if (TeamLocal.IsPlayer)
                        {
                            Console.WriteLine($"Subtracted {effect.Value} from Cost of {person.Name}");
                        }
                        break;
                    
                    default:
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
                                if (TeamLocal.IsPlayer)
                                {
                                    Console.WriteLine($"Multiplied {totalEffect / e.Value} by {e.Value} | Total Effect: {totalEffect}");
                                }
                            }
                            break;
                        
                        case "Multiply Position by Size":
                            int posSize = 0;
                            foreach (var p in TeamLocal.Players)
                            {
                                if (p.CurrentPositionID == e.Target)
                                {
                                    posSize++;
                                }
                            }
                            if (posSize > 0 && e.Target == person.CurrentPositionID)
                            {
                                totalEffect *= posSize;
                                if (TeamLocal.IsPlayer)
                                {
                                    Console.WriteLine($"Multiplied {totalEffect / posSize} by {posSize} | Total Effect: {totalEffect}");
                                }
                            }
                            break;

                        case "Add Value by Size":
                            int size = 0;
                            foreach (var p in TeamLocal.Players)
                            {
                                if (p.CurrentPositionID == e.Target)
                                {
                                    size++;
                                }
                            }
                            if (size > 0 && e.Target == person.CurrentPositionID)
                            {
                                totalEffect += size * e.Value;
                                if (TeamLocal.IsPlayer)
                                {
                                    Console.WriteLine($"Added {size * e.Value} to Value | Total Effect: {totalEffect}");
                                }
                            }
                            break;
                            
                    }
                }
            }
            return totalEffect;

        }
    }
}