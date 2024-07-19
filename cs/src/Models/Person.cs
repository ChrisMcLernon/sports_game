namespace sports_game.src.Models
{
    public struct Position
    {
        public string Name { get; set; }
        public double Modifier { get; set; }
        public int Size { get; set; }
    }
    public struct Person
    {
        public string Name { get; set; }
        public string Rarity { get; set; }
        public int Value { get; set; }
        public Position CurrentPosition { get; set; }
        public int Cost { get; set; }
        public List<Effect> Effects { get; set; }
        public string Status { get; set; }
    }
}
