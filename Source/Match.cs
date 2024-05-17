namespace SRkMatchmakerAPI.Framework;

public enum MatchResult { WL, DD, LW }

public class Match
{
    public MatchResult Result { get; private set; }

    public List<Player> Home { get; }
    public List<Player> Away { get; }
    public List<Player> All { get; }
    public int PassIndex { get; }

    public (int Min, int Max, int Avg, int HomeAvg, int AwayAvg, int PosHomeAvg, int PosAwayAvg) MMR { get; }
    public (float Avg, float HomeAvg, float AwayAvg) Skill { get; }

    public static List<PlayerPos> Formation = new List<PlayerPos>()
    {
        PlayerPos.GK,
        PlayerPos.CB,
        PlayerPos.CB,
        PlayerPos.FB,
        PlayerPos.FB,
        PlayerPos.DM,
        PlayerPos.LM,
        PlayerPos.LM,
        PlayerPos.AM,
        PlayerPos.WG,
        PlayerPos.ST
    };

    //static RandomNumberGenerator RNG;

    static Match()
    {
        //var RNG = new Random();
        //RNG.
        //RNG = new RandomNumberGenerator();
        //RNG.Randomize();
    }

    public static Match FromListOfPlayers(List<Player> players)
    {
        return new Match(new List<Player>(players.Take(11)), new List<Player>(players.Skip(11).Take(11)), 0);
    }

    public Match(List<Player> home, List<Player> away, int passIndex)
    {
        Home = home;
        Away = away;
        All = new List<Player>();

        PassIndex = passIndex;

        All.AddRange(Home);
        All.AddRange(Away);

        var H = 0f;
        var A = 0f;

        for (var i = 0; i < 11; i++)
        {
            var h = Home[i];
            var a = Away[i];

            H += h.GetMMRInPos(Formation[i]);
            A += a.GetMMRInPos(Formation[i]);
        }

        MMR = (All.Min(p => p.MMR), All.Max(p => p.MMR), (int)All.Average(p => p.MMR),
            (int)Home.Average(p => p.MMR), (int)Away.Average(p => p.MMR), (int)(H / 11f), (int)(A / 11f));

        var allAvg = (float)All.Average(p => Math.Pow(100 * p.Skill, 4));
        var homeAvg = (float)Home.Average(p => Math.Pow(100 * p.Skill, 4));
        var awayAvg = (float)Away.Average(p => Math.Pow(100 * p.Skill, 4));

        Skill = (allAvg, homeAvg, awayAvg);
        //Skill = (
        //    ),
        //    ,
        //    ;
    }

    const int BASE = 1000;

    T GetOutcome<T>(string otcof, params (T Outcome, float Weight)[] ocs)
    {
        var combinedWeight = ocs.Sum(p => p.Weight);

        var odd = RNG.RandiRange(0, (int)combinedWeight);

        T outcome = default;

        var sum = 0f;

        var isDefined = false;

        //DLSection($"{otcof}", odd, (int)combinedWeight);

        foreach (var p in ocs)
        {
            var chance = Math.Round(p.Weight / combinedWeight * 100f);

            //DLPrint($"{p.Item1} <= {Math.Round(sum + p.Item2)} ({chance}%)");

            if (!isDefined && odd <= sum + p.Weight)
            {
                outcome = p.Outcome;
                isDefined = true;

                //if (!Match.IsDebugging)
                //    break;
            }

            sum += p.Weight;
        }

        //DLPrint($"--- Outcome: {outcome} ---", true);

        return outcome;
    }

    public static Dictionary<T, float> GetOutcomeChance<T>(string otcof, params (T Outcome, float Weight)[] ocs)
    {
        var combinedWeight = ocs.Sum(p => p.Weight);


        // var odd = RNG.RandiRange(0, (int)combinedWeight);

        //T outcome = default;

        // var sum = 0f;

        //var isDefined = false;

        Dictionary<T, float> dict = new Dictionary<T, float>();

        //DLSection($"{otcof}", odd, (int)combinedWeight);

        foreach (var p in ocs)
        {
            //var chance = Mathf.Round(p.Weight / combinedWeight * 100f);

            dict.Add(p.Outcome, p.Weight / combinedWeight);
        }


        //DLPrint($"--- Outcome: {outcome} ---", true);

        return dict;
    }
}
