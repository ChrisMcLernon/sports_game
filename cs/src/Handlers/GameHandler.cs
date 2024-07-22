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
        private string? Seed { get; set; }
        public Random? SetRandom { get; set; }
        public int Round { get; set; } = 1;
        public int Losses { get; set; } = 0;
        public CategoryService? PlayerCategoryService { get; set; }
        public CategoryService? StaffCategoryService { get; set; }
        public int Wins { get; private set; } = 0;

        public void GameLoop()
        {
            StartGame();
            PlayGame();
        }

        public void StartGame()
        {
            Console.Clear();
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

        private Sport PickSport()
        {
            while (true)
            {
                string input = InputReader.ReadText("Pick a Sport (0. Exit | 1. Football): ");

                switch (input)
                {
                    case "1":
                        Sport chosenSport = JsonReader.Read<Sport>("Football_Sport_Info");
                        return chosenSport;
                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }

            }


        }

        public void InitializeData()
        {
            AvailablePlayers = JsonReader.Read<List<Person>>("Football_Player_Stats");
            AvailableStaff = JsonReader.Read<List<Person>>("Football_Staff_Stats");
            TeamNames = JsonReader.Read<List<string>>("Team_Names");

            Console.WriteLine(AvailablePlayers.Count);
            Console.WriteLine(AvailableStaff.Count);
            Console.WriteLine(TeamNames.Count);

            Round = 1;
            Losses = 0;
            Wins = 0;

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
            Sport chosenSport = PickSport();
            PlayerTeam = new Team(teamName, chosenSport, "_", true);
            PlayerTeam.CurrentSport.GeneratePositions();

            while (PlayerTeam.Players.Count < PlayerTeam.CurrentSport.TeamSize && AvailablePlayers.Count > 0)
            {
                Person player = PlayerCategoryService.PickItem();
                PlayerTeam.AddPerson(player, false);
                RemoveAvailablePerson(player);
            }

            foreach (var player in PlayerTeam.Players)
            {
                player.PrintInfo();
            }

            while (PlayerTeam.Staff.Count < PlayerTeam.CurrentSport.StaffSize && AvailableStaff.Count > 0)
            {
                Person staff = StaffCategoryService.PickItem();
                PlayerTeam.AddPerson(staff, false);
                RemoveAvailablePerson(staff);
            }

            Person benchedPlayer = PlayerCategoryService.PickItem();
            PlayerTeam.AddPerson(benchedPlayer, true);
            RemoveAvailablePerson(benchedPlayer);

            Person benchedStaff = StaffCategoryService.PickItem();
            PlayerTeam.AddPerson(benchedStaff, true);
            RemoveAvailablePerson(benchedStaff);
            
            Console.WriteLine($"{PlayerTeam.Name} Created!");
            Console.WriteLine($"{AvailablePlayers.Count} Players Left | {AvailableStaff.Count} Staff Left | {PlayerTeam.Players.Count} Players | {PlayerTeam.Staff.Count} Staff | {PlayerTeam.BenchedPlayers.Count} Benched Players | {PlayerTeam.BenchedStaff.Count} Benched Staff");
        }

        public void PlayGame()
        {
            if (MarketHandlerLocal is null || PlayerTeam is null || EditorHandlerLocal is null)
            {
                throw new Exception("Some Data not Initialized");
            }
            else if (AvailablePlayers.Count == 0 || AvailableStaff.Count == 0)
            {
                foreach (var player in PlayerTeam.Players)
                {
                    player.PrintInfo();
                }

                Console.WriteLine("No Players or Staff Available");
                Close();
            }
            while (true)
            {
                Console.WriteLine("\n\nWhat would you like to do?");
                Console.WriteLine("1. Team Editor");
                Console.WriteLine("2. Visit Market");
                Console.WriteLine("3. Continue to Next Match");
                Console.WriteLine("0. Exit");
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
            int opponentScore = (int)(score[1] * (Round * 0.5));

            if (playerScore > opponentScore)
            {
                Console.WriteLine("You Win!");
                PlayerTeam.Budget += 100;
                Console.WriteLine($"\n\n{PlayerTeam.Name}: {playerScore} - {OpponentTeam.Name}: {opponentScore}");
                Wins++;
                if (Wins == 10)
                {
                    Console.WriteLine("You Win the Game!");
                    Console.Clear();
                    Close();
                }
            }
            else if (playerScore < opponentScore)
            {
                Console.WriteLine("You Lose!");
                PlayerTeam.Budget += 25;
                Console.WriteLine($"\n\n{PlayerTeam.Name}: {playerScore} - {OpponentTeam.Name}: {opponentScore}");
                Losses++;
                if (Losses == 3)
                {
                    Console.Clear();
                    Console.WriteLine("Game Over!");
                    Close();
                }
            }
            else
            {
                Console.WriteLine("Draw!");
                PlayerTeam.Budget += 50;
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

            OpponentTeam = new Team(TeamNames[SetRandom.Next(TeamNames.Count)], PlayerTeam.CurrentSport);
            OpponentTeam.CurrentSport.GeneratePositions();

            while (OpponentTeam.Players.Count < OpponentTeam.CurrentSport.TeamSize && AvailablePlayers.Count > 0)
            {
                Person player = PlayerCategoryService.PickItem();
                OpponentTeam.AddPerson(player, false);
                RemoveAvailablePerson(player);
            }

            while (OpponentTeam.Staff.Count < OpponentTeam.CurrentSport.StaffSize && AvailableStaff.Count > 0)
            {
                Person staff = StaffCategoryService.PickItem();
                OpponentTeam.AddPerson(staff, false);
                RemoveAvailablePerson(staff);
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

            if (PlayerTeam.CurrentSport.PossiblePlayerPositions.Contains(person.CurrentPosition))
            {
                AvailablePlayers.Add(person);
                PlayerCategoryService.AddItem(person);
            }
            else
            {
                AvailableStaff.Add(person);
                StaffCategoryService.AddItem(person);
            }
        }

        public void RemoveAvailablePerson(Person person)
        {
            if (PlayerTeam is null)
            {
                throw new Exception("Some Data not Initialized");
            }

            if (PlayerTeam.CurrentSport.PossiblePlayerPositions.Contains(person.CurrentPosition))
            {
                AvailablePlayers.Remove(person);
                PlayerCategoryService.RemoveItem(person);
            }
            else
            {
                AvailableStaff.Remove(person);
                StaffCategoryService.RemoveItem(person);
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
                player.PrintInfo();
                unroundedPlayerValue += player.Value;
                unroundedPlayerValue += PlayerTeam.EffectHandlerTeam.ApplyPersonEffects(player);
                Console.WriteLine($"Player Value with effects: {unroundedPlayerValue}");
                unroundedPlayerValue *= player.CurrentPosition.Modifier;
                Console.Write($"Multipled by {player.CurrentPosition.Modifier} adding {Convert.ToInt32(Math.Round(unroundedPlayerValue))}\n");

                totalPoints[0] += Convert.ToInt32(Math.Round(unroundedPlayerValue));
                Console.WriteLine($"Added: {Convert.ToInt32(Math.Round(unroundedPlayerValue))} | Total Points: {totalPoints[0]}\n\n");
                unroundedPlayerValue = 0;
            }
            foreach (var staff in PlayerTeam.Staff)
            {
                staff.PrintInfo();
                unroundedPlayerValue += staff.Value;
                unroundedPlayerValue += PlayerTeam.EffectHandlerTeam.ApplyPersonEffects(staff);
                Console.WriteLine($"Staff Value with effects: {unroundedPlayerValue}\n");
                unroundedPlayerValue *= staff.CurrentPosition.Modifier;
                Console.Write($"Multipled by {staff.CurrentPosition.Modifier} adding {Convert.ToInt32(Math.Round(unroundedPlayerValue))}\n");

                totalPoints[0] += Convert.ToInt32(Math.Round(unroundedPlayerValue));
                Console.WriteLine($"Added: {Convert.ToInt32(Math.Round(unroundedPlayerValue))} | Total Points: {totalPoints[0]}\n");
                unroundedPlayerValue = 0;
            }

            foreach (var enemy in OpponentTeam.Players)
            {
                enemy.PrintInfo();
                unroundedEnemyValue += enemy.Value;
                unroundedEnemyValue += OpponentTeam.EffectHandlerTeam.ApplyPersonEffects(enemy);
                Console.WriteLine($"Player Value with effects: {unroundedEnemyValue}");
                unroundedEnemyValue *= enemy.CurrentPosition.Modifier;
                Console.Write($"Multipled by {enemy.CurrentPosition.Modifier} adding {Convert.ToInt32(Math.Round(unroundedEnemyValue))}\n");

                totalPoints[1] += Convert.ToInt32(Math.Round(unroundedEnemyValue));
                Console.WriteLine($"Added: {Convert.ToInt32(Math.Round(unroundedEnemyValue))} | Total Points: {totalPoints[1]}\n");
                unroundedEnemyValue = 0;

            }
            foreach (var staff in OpponentTeam.Staff)
            {
                staff.PrintInfo();
                unroundedEnemyValue += staff.Value;
                unroundedEnemyValue += OpponentTeam.EffectHandlerTeam.ApplyPersonEffects(staff);
                Console.WriteLine($"Staff Value with effects: {unroundedEnemyValue}\n");
                unroundedEnemyValue *= staff.CurrentPosition.Modifier;
                Console.Write($"Multipled by {staff.CurrentPosition.Modifier} adding {Convert.ToInt32(Math.Round(unroundedEnemyValue))}\n");
                
                totalPoints[1] += Convert.ToInt32(Math.Round(unroundedEnemyValue));
                Console.WriteLine($"Added: {Convert.ToInt32(Math.Round(unroundedEnemyValue))} | Total Points: {totalPoints[1]}\n");
                unroundedEnemyValue = 0;
            }

            return totalPoints;
        }
    }
}