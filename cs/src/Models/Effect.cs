namespace sports_game.src.Models
{
    public class Effect
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public string Target { get; set; }

        public Effect(string name, string description, int value, string target = "")
        {
            Name = name;
            Description = description;
            Value = value;
            Target = target;
        }
    }
}
