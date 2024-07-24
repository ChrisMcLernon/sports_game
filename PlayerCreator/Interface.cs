namespace PlayerCreator
{
    public class Interface
    {
        public static Dictionary<string, string> PositionDictionary = new Dictionary<string, string>
        {
            { "QUARTERBACK", "PP-000001"},
            { "RUNNING_BACK", "PP-000002" },
            { "WIDE_RECEIVER", "PP-000003" },
            { "TIGHT_END", "PP-000004" },
            { "OFFENSIVE_LINE", "PP-000005" },
            { "DEFENSIVE_LINE", "PP-000006" },
            { "LINEBACKER", "PP-000007" },
            { "DEFENSIVE_BACK", "PP-000008" },
            { "KICKER", "PP-000009" },
            { "ALL_ROUNDER", "PP-000010" },
            { "HEAD_COACH", "SP-000001" },
            { "OFFENSIVE_COORDINATOR", "SP-000002" },
            { "DEFENSIVE_COORDINATOR", "SP-000003" },
            { "SPECIAL_TEAMS_COACH", "SP-000004" },
            { "TRAINER", "SP-000005" },
            { "EQUIPMENT_MANAGER", "SP-000006" },
            { "MASTERMIND", "SP-000007" },
        };

        public static Person CreatePlayer()
        {
            Console.WriteLine("Enter the player's name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter the player's rarity:");
            string rarity = "";
            while (true)
            {
                rarity = Console.ReadLine().ToUpper();
                if (rarity == "COMMON" || rarity == "UNCOMMON" || rarity == "RARE" || rarity == "EPIC" || rarity == "LEGENDARY")
                {
                    break;
                }
                Console.WriteLine("Invalid rarity. Please enter a valid rarity (Common, Uncommon, Rare, Epic, or Legendary):");
            }
            List<Person> people = JsonService.Read<List<Person>>("Football_Player_Stats");
            string ID = "";
            for (int i = 0; i < people.Count; i++)
            {
                if (i == people.Count - 1)
                {
                    ID = people[i].ID.Substring(0, people[i].ID.Length - 1) + (int.Parse(people[i].ID.Substring(people[i].ID.Length - 1)) + 1);
                }
            }
            Console.WriteLine(ID);
            Console.WriteLine("Enter the player's value:");
            int value = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the player's current position ID:");
            string currentPositionID;
            while (true)
            {
                currentPositionID = Console.ReadLine().ToUpper();
                if (PositionDictionary.ContainsKey(currentPositionID) && PositionDictionary[currentPositionID].Substring(0, 2) == "PP")
                {
                    currentPositionID = PositionDictionary[currentPositionID];
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid position ID. Please enter a valid position ID (PP-000001, PP-000002, PP-000003, PP-000004, PP-000005, PP-000006, PP-000007, PP-000008, PP-000009, PP-000010):");
                }
            }
            Console.WriteLine("Enter the player's cost:");
            int cost = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the number of effects:");
            int effectCount = int.Parse(Console.ReadLine());
            List<Effect> effects = [];
            for (int i = 0; i < effectCount; i++)
            {
                Console.WriteLine("Enter the effect name:");
                string effectName = Console.ReadLine();
                Console.WriteLine("Enter the effect description:");
                string effectDescription = Console.ReadLine();
                Console.WriteLine("Enter the effect value:");
                int effectValue = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter the effect target:");
                string effectTarget = "";
                while (true)
                {
                    effectTarget = Console.ReadLine().ToUpper();
                    if (PositionDictionary.ContainsKey(effectTarget))
                    {
                        effectTarget = PositionDictionary[effectTarget];
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid position ID. Please enter a valid position ID (PP-000001, PP-000002, PP-000003, PP-000004, PP-000005, PP-000006, PP-000007, PP-000008, PP-000009, PP-000010):");
                    }
                }
                effects.Add(new Effect(effectName, effectDescription, effectValue, effectTarget));
            }
            Console.WriteLine("Enter the player's status:");
            string status = "";
            while (true)
            {
                status = Console.ReadLine().ToUpper();
                if (status == "ACTIVE" || status == "INJURED")
                {
                    break;
                }
                Console.WriteLine("Invalid status. Please enter a valid status (Active, Injured, Suspended, or Retired):");
            
            }

            Person player = new Person(name, rarity, ID, value, currentPositionID, cost, effects, status);
            return player;
        }

        public static Person CreateStaff()
        {
            Console.WriteLine("Enter the staff's name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter the staff's rarity:");
            string rarity = "";
            while (true)
            {
                rarity = Console.ReadLine().ToUpper();
                if (rarity == "COMMON" || rarity == "UNCOMMON" || rarity == "RARE" || rarity == "EPIC" || rarity == "LEGENDARY")
                {
                    break;
                }
                Console.WriteLine("Invalid rarity. Please enter a valid rarity (Common, Uncommon, Rare, Epic, or Legendary):");
            }
            List<Person> people = JsonService.Read<List<Person>>("Football_Staff_Stats");
            string ID = "";
            for (int i = 0; i < people.Count; i++)
            {
                if (i == people.Count - 1)
                {
                    ID = people[i].ID.Substring(0, people[i].ID.Length - 1) + (int.Parse(people[i].ID.Substring(people[i].ID.Length - 1)) + 1);
                }
            }
            Console.WriteLine(ID);
            Console.WriteLine("Enter the staff's value:");
            int value = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the staff's current position ID:");
            string currentPositionID;
            while (true)
            {
                currentPositionID = Console.ReadLine().ToUpper();
                if (PositionDictionary.ContainsKey(currentPositionID) && PositionDictionary[currentPositionID].Substring(0, 2) == "SP")
                {
                    currentPositionID = PositionDictionary[currentPositionID];
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid position ID. Please enter a valid position ID (SP-000001, SP-000002, SP-000003, SP-000004, SP-000005, SP-000006, SP-000007):");
                }
            }
            Console.WriteLine("Enter the staff's cost:");
            int cost = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the number of effects:");
            int effectCount = int.Parse(Console.ReadLine());
            List<Effect> effects = [];
            for (int i = 0; i < effectCount; i++)
            {
                Console.WriteLine("Enter the effect name:");
                string effectName = Console.ReadLine();
                Console.WriteLine("Enter the effect description:");
                string effectDescription = Console.ReadLine();
                Console.WriteLine("Enter the effect value:");
                int effectValue = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter the effect target:");
                string effectTarget = "";
                while (true)
                {
                    effectTarget = Console.ReadLine().ToUpper();
                    if (PositionDictionary.ContainsKey(effectTarget))
                    {
                        effectTarget = PositionDictionary[effectTarget];
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid position ID. Please enter a valid position ID \n(PP-000001, PP-000002, PP-000003, PP-000004, PP-000005, PP-000006, PP-000007, PP-000008, PP-000009, PP-000010SP-000001, SP-000002, SP-000003, SP-000004, SP-000005, SP-000006, SP-000007):");
                    }
                }
                effects.Add(new Effect(effectName, effectDescription, effectValue, effectTarget));
            }
            Console.WriteLine("Enter the staff's status:");
            string status = "";
            while (true)
            {
                status = Console.ReadLine().ToUpper();
                if (status == "ACTIVE" || status == "INJURED")
                {
                    break;
                }
                Console.WriteLine("Invalid status. Please enter a valid status (Active, Injured, Suspended, or Retired):");
            
            }

            Person player = new Person(name, rarity, ID, value, currentPositionID, cost, effects, status);
            return player;
        }
    }
}
