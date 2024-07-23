namespace sports_game.src.Models
{
    public class Position
    {
        public string Name { get; set; }
        public double Modifier { get; set; }
        public string ID { get; set; }
    }
    public class Person
    {

        public string Name { get; set; }
        public string Rarity { get; set; }
        public string ID { get; set; }
        public int Value { get; set; }
        public string CurrentPositionID { get; set; }
        public Position CurrentPosition { get; set; }
        public int Cost { get; set; }
        public List<Effect> Effects { get; set; }
        public string Status { get; set; }

        public Person(string name, string rarity, int value, string currentPositionID, int cost, List<Effect> effects, string status)
        {
            Name = name;
            Rarity = rarity;
            Value = value;
            CurrentPositionID = currentPositionID;
            Cost = cost;
            Effects = effects;
            Status = status;       
        }

        public void PrintInfo()
        {
            Console. WriteLine($"Name: {Name} | Rarity: {Rarity} | Value: {Value} | Cost: ${Cost} | Status: {Status} | Position: {CurrentPosition.Name}");
        }
    }
}
