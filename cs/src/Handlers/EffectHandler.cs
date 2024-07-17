using System.Security;
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
    }
}