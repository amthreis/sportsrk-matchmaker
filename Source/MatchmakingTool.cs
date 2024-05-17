using SRkMatchmakerAPI.Framework.DTO;

namespace SRkMatchmakerAPI.Framework;

public class MatchmakingTool
{
    List<Player> players;
    SortedSet<Player> queue;
    Dictionary<PlayerPos, SortedSet<Player>> posQueue = new Dictionary<PlayerPos, SortedSet<Player>>();


    SortedSet<Player> queueALGO2;
    Dictionary<PlayerPos, SortedSet<Player>> posQueueALGO2 = new Dictionary<PlayerPos, SortedSet<Player>>();

    //blic static RandomNumberGenerator RNG;

    //Dictionary<Player, UIListPlayer> playerListItem = new Dictionary<Player, UIListPlayer>();

    PlayerDTO[] dtos;

    public MatchmakingTool(PlayerDTO[] dtos)
    {
        this.dtos = dtos;
    }

    public void Start(Action<MyResponse> callback)
    {
        Console.WriteLine("---- starting Matchmaking Tool. Players: ", dtos.Length);

        players = new List<Player>();

        foreach (var d in dtos)
        {
            var p = new Player(d.User.Id, d.User.Email, d.MMR, d.Pos);
            players.Add(p);
        }

        players = players.OrderByDescending(p => 1_000_000 + p.MMR).ToList();

        queue = new SortedSet<Player>(players, new PlayerComparer());

        foreach (PlayerPos pos in System.Enum.GetValues(typeof(PlayerPos)))
        {
            posQueue.Add(pos, new SortedSet<Player>(players, new PlayerComparerByPos(pos)));

            var f = posQueue[pos].First();

            Console.WriteLine($"best {pos}: {f.Username} (${f.Pos }, MMR: {f.MMR})");
        }

        while(true)
        {
            if (!Make1Game(queue, posQueue))
            {
                break;
            }
        }

        Console.WriteLine($"Done! {players.Count} players (total), {matches.Count} matches arranged, {queue.Count} unmatched players.");

        foreach(var m in matches)
        {
            matchOTDs.Add(new MatchOTD(m));
        }

        callback?.Invoke(new MyResponse(matchOTDs, queue.Select(q => q.Id).ToList()));
    }

    List<MatchOTD> matchOTDs = new List<MatchOTD>();

    List<Match> matches = new List<Match>();

    bool Make1Game(SortedSet<Player> Q, Dictionary<PlayerPos, SortedSet<Player>> posQ)
    {
        var matchPlayers = new List<Player>();

        Console.WriteLine("----------------------- Searching...");
        while (matchPlayers.Count < 22 && Q.Count > 0)
        {
            var ply = PopFromPosQueue(Match.Formation[matchPlayers.Count % 11]);

            if (ply != null)
            {
                matchPlayers.Add(ply);
            }
            else break;
        }

        Player PopFromPosQueue(PlayerPos pos)
        {
            Player selected = null;

            foreach (var q in posQ[pos])
            {
                selected = q;
                break;
            }

            if (selected != null)
            {
                Console.WriteLine($"Pop from queue({pos}): {selected.Username}");
                //queue.Remove(selected);
                RemoveFromQueues(selected);

            }

            return selected;
        }

        void RemoveFromQueues(Player p)
        {
            Q.Remove(p);

            foreach (PlayerPos pos in System.Enum.GetValues(typeof(PlayerPos)))
            {
                posQ[pos].Remove(p);
            }
        }

        Player PopFromQueue()
        {
            Player selected = null;

            foreach (var q in queue)
            {
                selected = q;
                break;
            }

            if (selected != null)
            {
                Console.WriteLine($"Pop from queue: {selected.Username}");
                Q.Remove(selected);
            }

            return selected;
        }

        Console.WriteLine($"Found {matchPlayers.Count}/22");

        if (matchPlayers.Count < 22)
        {
            foreach(var p in matchPlayers)
            {
                queue.Add(p);
            }

            Console.WriteLine("Couldn't make a game!");
            return false;
        }
        else
        {
            //foreach (var p in matchPlayers)
            //{
            //    playerListItem[p].Disable();
            //}

            var m = Match.FromListOfPlayers(matchPlayers);

            matches.Add(m);

            return true;
            //GenerateMatchUI(m, matchList);
        }
    }
}
