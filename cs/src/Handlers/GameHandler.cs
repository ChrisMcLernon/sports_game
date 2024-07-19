using System.ComponentModel;
using sports_game.src.Entities;
using sports_game.src.Models;
using sports_game.src.Services;

namespace sports_game.src.Handlers
{
    public class GameHandler()
    {
        public Team? PlayerTeam { get; set; }
        static private Team? OpponentTeam { get; set; }
        public List<Person> AvailablePlayers { get; set; } = [];
        public List<Person> AvailableStaff { get; set; } = [];
        static private List<string>? TeamNames { get; set; }
        static private MarketHandler? MarketHandlerLocal { get; set; }
        static private EditorHandler? EditorHandlerLocal { get; set; }
        private string Seed { get; set; }
        public Random? SetRandom { get; set; }
        static private int TeamSize { get; } = 5;
        static private int StaffSize { get; } = 3;
        public int Round { get; set; } = 1;
        public CategoryService PlayerCategoryService { get; set; }
        public CategoryService StaffCategoryService { get; set; }

        public void GameLoop()
        {
            StartGame();
            PlayGame();
        }

        public void StartGame()
        {
            Console.WriteLine("Welcome to '_' (0 to exit | 1 to start game)");
            string input = InputReader.ReadText();
            if (input == "0")
            {
                Close();
            }
            else if (input == "1")
            {
                InitializeData();
                SetSeed();
                GenerateStarterTeam();
            }
            else
            {
                Console.WriteLine("Invalid Input");
                StartGame();
            }
        }

        public void InitializeData()
        {
            AvailablePlayers = JsonReader.Read<List<Person>>("Football_Player_Stats");
            AvailableStaff = JsonReader.Read<List<Person>>("Football_Staff_Stats");
            TeamNames = JsonReader.Read<List<string>>("Team_Names");

            if (AvailablePlayers is null || AvailableStaff is null || TeamNames is null)
            {
                throw new Exception("Some Data not Initialized");
            }

            var playerCategoryCommon = new Category("COMMON", 0.7);
            var playerCategoryUncommon = new Category("UNCOMMON", 0.3);
            var playerCategoryRare = new Category("RARE", 0.2);
            var playerCategoryEpic = new Category("EPIC", 0.1);
            var playerCategoryLegendary = new Category("LEGENDARY", 0.01);
            var staffCategoryCommon = new Category("COMMON", 0.7);
            var staffCategoryUncommon = new Category("UNCOMMON", 0.3);
            var staffCategoryRare = new Category("RARE", 0.2);
            var staffCategoryEpic = new Category("EPIC", 0.1);
            var staffCategoryLegendary = new Category("LEGENDARY", 0.01);

            Console.WriteLine($"{AvailablePlayers[0].Rarity} | {AvailablePlayers[0].Name}");

            PlayerCategoryService = new CategoryService(Convert.ToInt32(Seed));
            StaffCategoryService = new CategoryService(Convert.ToInt32(Seed));

            PlayerCategoryService.AddCategory(playerCategoryCommon);
            PlayerCategoryService.AddCategory(playerCategoryUncommon);
            PlayerCategoryService.AddCategory(playerCategoryRare);
            PlayerCategoryService.AddCategory(playerCategoryEpic);
            PlayerCategoryService.AddCategory(playerCategoryLegendary);

            StaffCategoryService.AddCategory(staffCategoryCommon);
            StaffCategoryService.AddCategory(staffCategoryUncommon);
            StaffCategoryService.AddCategory(staffCategoryRare);
            StaffCategoryService.AddCategory(staffCategoryEpic);
            StaffCategoryService.AddCategory(staffCategoryLegendary);

            PlayerCategoryService.DistributeItems(AvailablePlayers);
            StaffCategoryService.DistributeItems(AvailableStaff);

            MarketHandlerLocal = new(this);
            EditorHandlerLocal = new(this);
        }

        public void SetSeed()
        {
            Console.Write("Enter Seed: ");
            Seed = InputReader.ReadText();
            ConfigSeed(Seed);
        }

        public void ConfigSeed(string seed)
        {
            int calculatedSeed = 0;
            foreach (char c in seed)
            {
                calculatedSeed += Convert.ToInt32(c);
            }
            SetRandom = new Random(calculatedSeed);
        }

        public void GenerateStarterTeam()
        {
            if (SetRandom is null || TeamNames is null)
            {
                throw new Exception("Some Data not Initialized");
            }

            AvailablePlayers = [.. AvailablePlayers.OrderBy(x => SetRandom.Next())];
            string teamName = InputReader.ReadText("Enter Team Name: ");
            PlayerTeam = new Team(teamName);
            PlayerTeam.GeneratePossiblePositions();

            while (PlayerTeam.Players.Count < TeamSize && AvailablePlayers.Count > 0)
            {
                Person player = AvailablePlayers[SetRandom.Next(AvailablePlayers.Count)];
                if (!PlayerTeam.PositionFilled(player.CurrentPosition))
                {
                    PlayerTeam.AddPerson(player);
                    AvailablePlayers.Remove(player);
                }
            }

            while (PlayerTeam.Staff.Count < StaffSize && AvailableStaff.Count > 0)
            {
                Person staff = AvailableStaff[SetRandom.Next(AvailableStaff.Count)];
                if (!PlayerTeam.PositionFilled(staff.CurrentPosition))
                {
                    PlayerTeam.AddPerson(staff);
                    AvailableStaff.Remove(staff);
                }
            }

            Person benchedPlayer = AvailablePlayers[SetRandom.Next(AvailablePlayers.Count)];
            PlayerTeam.BenchedPlayers.Add(benchedPlayer);
            AvailablePlayers.Remove(benchedPlayer);
            Console.WriteLine("Benching Player: " + benchedPlayer.Name);

            Person benchedStaff = AvailableStaff[SetRandom.Next(AvailableStaff.Count)];
            PlayerTeam.BenchedStaff.Add(benchedStaff);
            AvailableStaff.Remove(benchedStaff);
            Console.WriteLine("Benching Staff: " + benchedStaff.Name);
        }

        public void PlayGame()
        {
            if (MarketHandlerLocal is null || PlayerTeam is null || EditorHandlerLocal is null)
            {
                throw new Exception("Some Data not Initialized");
            }

            while (true)
            {
                Console.WriteLine("\n\nWhat would you like to do?");
                Console.WriteLine("1. Team Editor");
                Console.WriteLine("2. Visit Market");
                Console.WriteLine("3. Continue to Next Match");
                Console.WriteLine("0. Exit\n");
                string input = InputReader.ReadText("Enter your choice: ");

                switch (input)
                {
                    case "0":
                        //Console.Clear();
                        Close();
                        break;
                    case "1":
                        //Console.Clear();
                        EditorHandlerLocal.EditorInterface();
                        break;
                    case "2":
                        //Console.Clear();
                        MarketHandlerLocal.MarketInterface();
                        break;
                    case "3":
                        //Console.Clear();
                        foreach (var c in PlayerCategoryService.Categories)
                        {
                            Console.WriteLine($"{c.Name} | {c.People.Count}");
                            foreach (var p in c.People)
                            {
                                Console.WriteLine(p.Name);
                            }
                        }
                        foreach (var c in StaffCategoryService.Categories)
                        {
                            Console.WriteLine($"{c.Name} | {c.People.Count}");
                            foreach (var p in c.People)
                            {
                                Console.WriteLine(p.Name);
                            }
                        }

                        PlayRound();
                        break;
                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }
            }
        }

        public void PlayRound()
        {
            GenerateOpponentTeam();

            if (PlayerTeam is null || OpponentTeam is null)
            {
                throw new Exception("Some Data not Initialized");
            }

            List<int> score = CalculateScore();
            int playerScore = score[0];
            int opponentScore = score[1];

            if (playerScore > opponentScore)
            {
                Console.WriteLine("You Win!");
                Console.WriteLine($"\n\n{PlayerTeam.Name}: {playerScore} - {OpponentTeam.Name}: {opponentScore}");
            }
            else if (playerScore < opponentScore)
            {
                Console.WriteLine("You Lose!");
                Console.WriteLine($"\n\n{PlayerTeam.Name}: {playerScore} - {OpponentTeam.Name}: {opponentScore}");
            }
            else
            {
                Console.WriteLine("Draw!");
                Console.WriteLine($"\n\n{PlayerTeam.Name}: {playerScore} - {OpponentTeam.Name}: {opponentScore}");
            }

            HandleOpponentTeam();
            HandleRoundEnd();
        }

        private void HandleRoundEnd()
        {
            if (PlayerTeam is null || OpponentTeam is null)
            {
                throw new Exception("Some Data not Initialized");
            }

            PlayerTeam.CalcInterest();
            OpponentTeam.CalcInterest();
            

            Round++;
            Console.WriteLine($"Round {Round} Complete!");
        }

        public void GenerateOpponentTeam()
        {
            if (SetRandom is null || TeamNames is null)
            {
                throw new Exception("Some Data not Initialized");
            }

            OpponentTeam = new Team(TeamNames[SetRandom.Next(TeamNames.Count)]);
            OpponentTeam.GeneratePossiblePositions();

            while (OpponentTeam.Players.Count < TeamSize && AvailablePlayers.Count > 0)
            {
                Person player = AvailablePlayers[SetRandom.Next(AvailablePlayers.Count)];
                if (!OpponentTeam.PositionFilled(player.CurrentPosition))
                {
                    OpponentTeam.AddPerson(player);
                    RemoveAvailablePerson(player);
                }
            }

            while (OpponentTeam.Staff.Count < StaffSize && AvailableStaff.Count > 0)
            {
                Person staff = AvailableStaff[SetRandom.Next(AvailableStaff.Count)];
                if (!OpponentTeam.PositionFilled(staff.CurrentPosition))
                {
                    OpponentTeam.AddPerson(staff);
                    RemoveAvailablePerson(staff);
                }
            }

        }

        public void HandleOpponentTeam()
        {
            if (OpponentTeam is null)
            {
                throw new Exception("Some Data not Initialized");
            }

            foreach (var player in OpponentTeam.Players)
            {
                AddAvailablePerson(player);
            }
            foreach (var staff in OpponentTeam.Staff)
            {
                AddAvailablePerson(staff);
            }
        }

        public void Close()
        {
            Console.WriteLine("Goodbye!");
            Environment.Exit(0);
        }

        public void AddAvailablePerson(Person person)
        {
            if (PlayerTeam is null)
            {
                throw new Exception("Some Data not Initialized");
            }

            if (PlayerTeam.PossiblePlayerPositions.Contains(person.CurrentPosition.Name))
            {
                AvailablePlayers.Add(person);
            }
            else
            {
                AvailableStaff.Add(person);
            }
        }

        public void RemoveAvailablePerson(Person person)
        {
            if (PlayerTeam is null || OpponentTeam is null)
            {
                throw new Exception("Some Data not Initialized");
            }

            if (PlayerTeam.PossiblePlayerPositions.Contains(person.CurrentPosition.Name))
            {
                AvailablePlayers.Remove(person);
            }
            else
            {
                AvailableStaff.Remove(person);
            }
        }

        public List<int> CalculateScore()
        {
            List<int> totalPoints = [0, 0];
            double unroundedPlayerValue = 0;
            double unroundedEnemyValue = 0;

            if (PlayerTeam is null || OpponentTeam is null)
            {
                throw new Exception("Some Data not Initialized");
            }

            foreach (var player in PlayerTeam.Players)
            {
                Console.WriteLine($"{player.Name} | {player.CurrentPosition.Name} | {player.CurrentPosition.Modifier} | {player.Value} ");
                unroundedPlayerValue += player.Value;
                unroundedPlayerValue += PlayerTeam.EffectHandlerTeam.ApplyPersonEffects(player);
                Console.WriteLine($"Player Value with effects: {unroundedPlayerValue}");
                unroundedPlayerValue *= player.CurrentPosition.Modifier;
                Console.Write($"Multipled by {player.CurrentPosition.Modifier} adding {Convert.ToInt32(Math.Round(unroundedPlayerValue))}\n");

                totalPoints[0] += Convert.ToInt32(Math.Round(unroundedPlayerValue));
                Console.WriteLine($"Added: {Convert.ToInt32(Math.Round(unroundedPlayerValue))} | Total Points: {totalPoints[0]}\n\n");
                unroundedPlayerValue = 0;
            }
            foreach (var enemy in OpponentTeam.Players)
            {
                Console.WriteLine($"{enemy.Name} | {enemy.CurrentPosition.Name} | {enemy.CurrentPosition.Modifier} | {enemy.Value} ");
                unroundedEnemyValue += enemy.Value;
                unroundedEnemyValue += OpponentTeam.EffectHandlerTeam.ApplyPersonEffects(enemy);
                Console.WriteLine($"Player Value with effects: {unroundedEnemyValue}");
                unroundedEnemyValue *= enemy.CurrentPosition.Modifier;
                Console.Write($"Multipled by {enemy.CurrentPosition.Modifier} adding {Convert.ToInt32(Math.Round(unroundedEnemyValue))}\n");

                totalPoints[1] += Convert.ToInt32(Math.Round(unroundedEnemyValue));
                Console.WriteLine($"Added: {Convert.ToInt32(Math.Round(unroundedEnemyValue))} | Total Points: {totalPoints[1]}\n");
                unroundedEnemyValue = 0;

            }

            return totalPoints;
        }
    }
}