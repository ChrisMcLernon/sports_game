// See https://aka.ms/new-console-template for more information
using PlayerCreator;

while (true)
{
    Console.WriteLine("Enter 'q' to quit or 'c' to create another player:");
    string input = Console.ReadLine().ToLower();
    if (input == "q")
    {
        break;
    }
    else if (input == "c")
    {
        Person player = Interface.CreatePlayer();
        List<Person> people = JsonService.Read<List<Person>>("Football_Player_Stats");

        people.Add(player);

        JsonService.Write("Football_Player_Stats", people);
    }
    else
    {
        Console.WriteLine("Invalid input. Please try again.");
    }
}
