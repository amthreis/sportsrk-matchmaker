namespace SRkMatchmakerAPI.Framework;

public class Lobby
{
    public Dictionary<PlayerPos, int> SpotsAvailable = new Dictionary<PlayerPos, int>();

    public List<Player> Players = new List<Player>();

    public Lobby()
    {
        foreach (var p in Match.Formation)
        {
            if (!SpotsAvailable.ContainsKey(p))
                SpotsAvailable[p] = 0;

            SpotsAvailable[p] += 2;
        }
    }

    public void Take(Player p, PlayerPos pos)
    {
        Players.Add(p);

        SpotsAvailable[pos]--;
    }
}
