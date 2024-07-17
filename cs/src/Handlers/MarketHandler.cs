using sports_game.src.Models;

namespace sports_game.src.Handlers
{
    public class MarketHandler(List<Person> availablePlayers, List<Person> availableStaff)
    {
        public List<Person> AvailablePlayers { get; set;} = availablePlayers;
        public List<Person> AvailableStaff { get; set;} = availableStaff;
    }
}