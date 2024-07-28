using System.ComponentModel;
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
                switch (effect.Name.ToLower())
                {
                    case "increase value":
                        person.Value += effect.Value;
                        if (TeamLocal.IsPlayer)
                        {
                            Console.WriteLine($"Added {effect.Value} to Value of {person.Name}");

                        }
                        break;
                    
                    case "decrease value":
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

                    case "increase cost":
                        person.Cost += effect.Value;
                        if (TeamLocal.IsPlayer)
                        {
                            Console.WriteLine($"Added {effect.Value} to Cost of {person.Name}");
                        }
                        break;
                    
                    case "decrease cost":
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

                    case "higher chance of injury":
                        if (person.Status != "Injured" && effect.Value >= TeamLocal.GameHandlerLocal.SetRandom.Next(0, 100))
                        {
                            person.Status = "Injured";
                            person.Value /= 2;
                            person.Cost /= 2;
                            if (TeamLocal.IsPlayer)
                            {
                                Console.WriteLine($"{person.Name} is now Injured");
                            }
                        }
                        break;

                    case "multiply value by value":
                        totalEffect *= effect.Value;
                        if (TeamLocal.IsPlayer)
                        {
                            Console.WriteLine($"Multiplied {totalEffect / effect.Value} by {effect.Value} | Total Effect: {totalEffect}");
                        }
                        break;

                    case "increase budget":
                        TeamLocal.Budget += effect.Value;
                        if (TeamLocal.IsPlayer)
                        {
                            Console.WriteLine($"Added {effect.Value} to Budget | Total Budget: {TeamLocal.Budget}");
                        }
                        break;
                    
                    case "decrease budget":
                        if (TeamLocal.Budget < 0)
                        {
                            TeamLocal.Budget = 0;
                            Console.WriteLine("Budget is 0");
                        }
                        else
                        {
                            TeamLocal.Budget -= effect.Value;
                            if (TeamLocal.IsPlayer)
                            {
                                Console.WriteLine($"Subtracted {effect.Value} from Budget | Total Budget: {TeamLocal.Budget}");
                            }
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
                    switch (e.Name.ToLower())
                    {
                        case "multiply position":
                            if (e.Target == person.CurrentPositionID)
                            {
                                totalEffect *= e.Value;
                                if (TeamLocal.IsPlayer)
                                {
                                    Console.WriteLine($"Multiplied {totalEffect / e.Value} by {e.Value} | Total Effect: {totalEffect}");
                                }
                            }
                            break;
                        
                        case "multiply position by size":
                            int posSize = 0;
                            foreach (var p in TeamLocal.Players)
                            {
                                if (p.CurrentPositionID == e.Target)
                                {
                                    posSize++;
                                }
                            }
                            foreach (var p in TeamLocal.Staff)
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

                        case "add value by size":
                            int size = 0;
                            foreach (var p in TeamLocal.Players)
                            {
                                if (p.CurrentPositionID == e.Target)
                                {
                                    size++;
                                }
                            }
                            foreach (var p in TeamLocal.Staff)
                            {
                                if (p.CurrentPositionID == e.Target)
                                {
                                    size++;
                                }
                            }
                            if (size > 0)
                            {
                                totalEffect += size * e.Value;
                                if (TeamLocal.IsPlayer)
                                {
                                    Console.WriteLine($"Added {size * e.Value} to Value | Total Effect: {totalEffect}");
                                }
                            }
                            break;
                        
                        case "higher chance of injury to position":
                            if (person.Status != "Injured" && e.Target == person.CurrentPositionID && e.Value >= TeamLocal.GameHandlerLocal.SetRandom.Next(0, 100))
                            {
                                person.Status = "Injured";
                                person.Value /= 2;
                                person.Cost /= 2;
                                if (TeamLocal.IsPlayer)
                                {
                                    Console.WriteLine($"{person.Name} is now Injured");
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