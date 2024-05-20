using SRkMatchmakerAPI.Framework;

namespace SRkMatchmakerAPI.Persistence
{
    public class MatchmakerDbContext
    {
        public Dictionary<string, Player> Players;

        public MatchmakerDbContext()
        {
            Players = new Dictionary<string, Player>();
        }
    }
}
