namespace sports_game.src.Models
{
    public struct Position
    {
        public string Name { get; set; }
        public double Modifier { get; set; }
        public int Size { get; set; }
    }
    public class Person
    {
        public string Name { get; set; }
        public string Rarity { get; set; }
        public int Value { get; set; }
        public Position CurrentPosition { get; set; }
        public int Cost { get; set; }
        public List<Effect> Effects { get; set; }
        public string Status { get; set; }

        public void PrintInfo()
        {
            Console. WriteLine($"Name: {Name} | Rarity: {Rarity} | Value: {Value} | Position: {CurrentPosition.Name} | Modifier: {CurrentPosition.Modifier} | Cost: ${Cost}");
        }
    }
}
