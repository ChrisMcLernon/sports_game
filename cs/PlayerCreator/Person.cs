namespace PlayerCreator;
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

    public Person(string name, string rarity, string iD, int value, string currentPositionID, int cost, List<Effect> effects, string status)
    {
        Name = name;
        Rarity = rarity;
        ID = iD;
        Value = value;
        CurrentPositionID = currentPositionID;
        Cost = cost;
        Effects = effects;
        Status = status;       
    }

    public void PrintInfo()
    {
        Console. WriteLine($"Name: {Name} | Rarity: {Rarity} | Value: {Value} | Cost: ${Cost} | Status: {Status} | Position: {CurrentPosition.Name}");

        foreach (Effect effect in Effects)
        {
            Console.WriteLine($"Effect: {effect.Name} | Description: {effect.Description} | Value: {effect.Value} | Target: {effect.Target}");
        }
    }
}
