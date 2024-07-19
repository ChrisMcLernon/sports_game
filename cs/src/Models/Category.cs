namespace sports_game.src.Models
{
    public class Category(string name, double weight)
    {
        public string Name { get; set; } = name;
        public double Weight { get; set; } = weight;
        public List<Person> People { get; set; } = [];
    }
}
