using sports_game.src.Services;

namespace sports_game.src.Models
{
    public class Sport
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int TeamSize { get; set; }
        public int StaffSize { get; set; }
        public List<Position> PossiblePlayerPositions { get; set; } = [];
        public List<Position> PossibleStaffPositions { get; set; } = [];

        public void GeneratePositions()
        {
            PossiblePlayerPositions = JsonReader.Read<List<Position>>($"{Name}_Player_Positions");
            PossibleStaffPositions = JsonReader.Read<List<Position>>($"{Name}_Staff_Positions");
        }
    }
}
