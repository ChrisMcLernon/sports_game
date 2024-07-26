using System.ComponentModel;
using sports_game.src.Entities;
using sports_game.src.Models;
using sports_game.src.Services;

namespace sports_game.src.Handlers
{
    public class GameHandler()
    {
        public Team? PlayerTeam { get; set; }
        public Team? OpponentTeam { get; set; }
        public List<Person> AvailablePlayers { get; set; } = [];
        public List<Person> AvailableStaff { get; set; } = [];
        static private List<string>? TeamNames { get; set; }
        static private MarketHandler? MarketHandlerLocal { get; set; }
        static private EditorHandler? EditorHandlerLocal { get; set; }
        static private PlanningHandler? PlanningHandlerLocal { get; set; }
        private string? Seed { get; set; }
        public Random? SetRandom { get; set; }
        public int Round { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public CategoryService? PlayerCategoryService { get; set; }
        public CategoryService? StaffCategoryService { get; set; }
        public int Wins { get; private set; } = 0;
        public Sport CurrentSport { get; set; }

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
                SetSeed();
                InitializeData();
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
                        Sport chosenSport = JsonService.Read<Sport>("Football_Sport_Info");
                        return chosenSport;
                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }

            }


        }

        public void InitializeData()
        {
            AvailablePlayers = JsonService.Read<List<Person>>("Football_Player_Stats");
            AvailableStaff = JsonService.Read<List<Person>>("Football_Staff_Stats");
            TeamNames = JsonService.Read<List<string>>("Team_Names");

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

            PlayerCategoryService = new CategoryService(SetRandom);
            StaffCategoryService = new CategoryService(SetRandom);

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
            PlanningHandlerLocal = new(this);
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

            Console.WriteLine($"Seed: {calculatedSeed}    {SetRandom.ToString()}   {SetRandom.Next()}  {SetRandom.Next()}   {SetRandom.Next()}");
        }

        public void GenerateStarterTeam()
        {
            string teamName = InputReader.ReadText("Enter Team Name: ");
            CurrentSport = PickSport();
            PlayerTeam = new Team(this, teamName, "_", true);
            CurrentSport.GeneratePositions();
            PlayerTeam.TeamDataInitialize();

            foreach (var player in AvailablePlayers)
            {
                AddPosition(player);
            }
            foreach (var staff in AvailableStaff)
            {
                AddPosition(staff);
            }

            while (PlayerTeam.Players.Count < CurrentSport.TeamSize && AvailablePlayers.Count > 0)
            {
                Person player = PlayerCategoryService.PickItem();
                PlayerTeam.AddPerson(player, false);
                RemoveAvailablePerson(player);
            }

            while (PlayerTeam.Staff.Count < CurrentSport.StaffSize && AvailableStaff.Count > 0)
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
            Console.WriteLine($"{PlayerTeam.Players.Count} Players | {PlayerTeam.Staff.Count} Staff | {PlayerTeam.BenchedPlayers.Count} Benched Players | {PlayerTeam.BenchedStaff.Count} Benched Staff");
        }

        public void AddPosition(Person p)
        {
            while (p.CurrentPosition == null)
            {
                foreach (var position in CurrentSport.PossiblePlayerPositions)
                {
                    if (position.ID == p.CurrentPositionID)
                    {
                        p.CurrentPosition = position;
                        break;
                    }
                }
                foreach (var position in CurrentSport.PossibleStaffPositions)
                {
                    if (position.ID == p.CurrentPositionID)
                    {
                        p.CurrentPosition = position;
                        break;
                    }
                }
            }
        }

        public void PlayGame()
        {
            while (true)
            {
                Console.WriteLine("\n\n\n\nWhat would you like to do?");
                Console.WriteLine($"{PlayerTeam.Name} | Budget: {PlayerTeam.Budget} | Wins: {Wins} | Losses: {Losses}");
                Console.WriteLine($"Players: {PlayerTeam.Players.Count} | Staff: {PlayerTeam.Staff.Count} | Benched Players: {PlayerTeam.BenchedPlayers.Count} | Benched Staff: {PlayerTeam.BenchedStaff.Count}");
                Console.WriteLine("1. Team Editor");
                Console.WriteLine("2. Continue to Match Planning");
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

            PlanningHandlerLocal.PlanningInterface();

            List<int> score = CalculateScore();

            int playerScore = score[0];
            int opponentScore = score[1];

            if (playerScore > opponentScore)
            {
                Console.WriteLine("You Win!");
                PlayerTeam.Budget += 100;
                Console.WriteLine($"\n\n{PlayerTeam.Name}: {playerScore} - {OpponentTeam.Name}: {opponentScore}");
                Wins++;
                if (Wins == 10)
                {
                    Console.Clear();
                    Console.WriteLine("You Win the Game!");
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

            foreach (var player in PlayerTeam.Players)
            {
                injuryChance(player);
            }
            foreach (var enemy in OpponentTeam.Players)
            {
                injuryChance(enemy);
            }

            HandleOpponentTeam();
            HandleRoundEnd();
        }

        private void injuryChance(Person player)
        {
            if (SetRandom.Next(100) < 10)
            {
                player.Status = "Injured";
                Console.WriteLine($"{player.Name} is Injured!");
                if (PlayerTeam.Players.Contains(player))
                {
                    PlanningHandlerLocal.injuredPlayers.Add(player);
                }
            }

            if (player.Status == "Injured")
            {
                player.Value /= 2;
                player.Cost /= 2;
            }
        }

        private void HandleRoundEnd()
        {

            PlayerTeam.CalcInterest();
            OpponentTeam.CalcInterest();

            PlanningHandlerLocal.CurrentLineup.Clear();

            Console.WriteLine($"Round {Round} Complete!");
            Round++;

            MarketHandlerLocal.MarketInterface();
        }

        public void GenerateOpponentTeam()
        {

            OpponentTeam = new Team(this, TeamNames[SetRandom.Next(TeamNames.Count)]);
            CurrentSport.GeneratePositions();
            OpponentTeam.TeamDataInitialize();

            while (OpponentTeam.Players.Count < CurrentSport.TeamSize && AvailablePlayers.Count > 0)
            {
                Person player = PlayerCategoryService.PickItem();
                OpponentTeam.AddPerson(player, false);
                RemoveAvailablePerson(player);
            }

            while (OpponentTeam.Staff.Count < CurrentSport.StaffSize && AvailableStaff.Count > 0)
            {
                Person staff = StaffCategoryService.PickItem();
                OpponentTeam.AddPerson(staff, false);
                RemoveAvailablePerson(staff);
            }

            Console.WriteLine($"Players: {PlayerTeam.Players.Count} | Staff: {PlayerTeam.Staff.Count} | Benched Players: {PlayerTeam.BenchedPlayers.Count} | Benched Staff: {PlayerTeam.BenchedStaff.Count}");


        }

        public void HandleOpponentTeam()
        {

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
            if (CurrentSport.PossiblePlayerPositions.ToList().Exists(p => p.ID == person.CurrentPositionID))
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
            if (CurrentSport.PossiblePlayerPositions.ToList().Exists(p => p.ID == person.CurrentPositionID))
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

        public List<int> CalculateScore(bool isPreview = false)
        {
            List<int> totalPoints = [0, 0];
            double unroundedPlayerValue = 0;
            double unroundedEnemyValue = 0;

            foreach (var enemy in OpponentTeam.Players)
            {
                OpponentTeam.EffectHandlerTeam.AddEffects(enemy);

                unroundedEnemyValue += OpponentTeam.EffectHandlerTeam.ApplyPersonEffects(enemy);
                unroundedEnemyValue *= enemy.CurrentPosition.Modifier;
                totalPoints[1] += Convert.ToInt32(Math.Round(unroundedEnemyValue));
                unroundedEnemyValue = 0;

            }
            foreach (var staff in OpponentTeam.Staff)
            {
                OpponentTeam.EffectHandlerTeam.AddEffects(staff);

                unroundedEnemyValue += OpponentTeam.EffectHandlerTeam.ApplyPersonEffects(staff);
                unroundedEnemyValue *= staff.CurrentPosition.Modifier;
                totalPoints[1] += Convert.ToInt32(Math.Round(unroundedEnemyValue));
                unroundedEnemyValue = 0;
            }

            if ((Round % 3) == 0)
            {
                totalPoints[1] = (int)(totalPoints[1] * Math.Round(Round * 0.5, 1));
            }
            else
            {
                totalPoints[1] = (int)(totalPoints[1] * Math.Round(Round * 0.2, 1));
            }

            if (isPreview)
            {
                return totalPoints;
            }

            while (PlanningHandlerLocal.CurrentLineup.Count != 0)
            {
                Person p = PlanningHandlerLocal.CurrentLineup.Pop();

                PlayerTeam.EffectHandlerTeam.AddEffects(p);

                p.PrintInfo();
                unroundedPlayerValue += PlayerTeam.EffectHandlerTeam.ApplyPersonEffects(p);
                unroundedPlayerValue *= p.CurrentPosition.Modifier;
                Console.WriteLine($"Total Points: {Convert.ToInt32(Math.Round(unroundedPlayerValue / p.CurrentPosition.Modifier))} multiplied by {p.CurrentPosition.Modifier} = {Convert.ToInt32(Math.Round(unroundedPlayerValue))}");

                totalPoints[0] += Convert.ToInt32(Math.Round(unroundedPlayerValue));
                unroundedPlayerValue = 0;
            }

            foreach (var p in PlayerTeam.Players)
            {
                PlayerTeam.EffectHandlerTeam.RemoveEffects (p);
            }
            foreach (var p in PlayerTeam.Staff)
            {
                PlayerTeam.EffectHandlerTeam.RemoveEffects(p);
            }

            return totalPoints;
        }
    }
}