using PlayerCreator;

Console.Clear();

while (true)
{
    Console.WriteLine("Enter 'q' to quit or 'c' to create another player or 's' to create another staff:");
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
    else if (input == "s")
    {
        Person staff = Interface.CreateStaff();
        List<Person> people = JsonService.Read<List<Person>>("Football_Staff_Stats");

        people.Add(staff);

        JsonService.Write("Football_Staff_Stats", people);
    }
    else
    {
        Console.WriteLine("Invalid input. Please try again.");
    }
}
