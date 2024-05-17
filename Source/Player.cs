using System.ComponentModel.DataAnnotations;

namespace SRkMatchmakerAPI.Framework;

public enum PlayerPos { GK, FB, CB, DM, LM, AM, WG, ST }

public class PlayerComparer : IComparer<Player>
{
    int IComparer<Player>.Compare(Player x, Player y)
    {
        var xMMR = 1_000_000 + x.MMR;
        var yMMR = 1_000_000 + y.MMR;

        if (xMMR != yMMR)
        {
            return y.MMR.CompareTo(x.MMR);
        }
        else
        {
            return y.Email.CompareTo(x.Email);
        }
    }
}

public class PlayerComparerByPos : IComparer<Player>
{
    PlayerPos pos;

    public PlayerComparerByPos(PlayerPos pos)
    {
        this.pos = pos;
    }

    int IComparer<Player>.Compare(Player x, Player y)
    {
        var xMMR = x.GetMMRInPos(pos);
        var yMMR = y.GetMMRInPos(pos);

        if (xMMR != yMMR)
        {
            return yMMR.CompareTo(xMMR);
        }
        else
        {
            return y.Email.CompareTo(x.Email);
        }
    }
}

public class Player
{
    public static Dictionary<PlayerPos, Dictionary<PlayerPos, float>> PosMMRx;

    static Player()
    {
        PosMMRx = new Dictionary<PlayerPos, Dictionary<PlayerPos, float>>();

        PosMMRx[PlayerPos.GK] = new Dictionary<PlayerPos, float>()
        {

        };

        PosMMRx[PlayerPos.FB] = new Dictionary<PlayerPos, float>()
        {
            { PlayerPos.CB, 0.8f  },
            { PlayerPos.LM, 0.9f }
        };

        PosMMRx[PlayerPos.CB] = new Dictionary<PlayerPos, float>()
        {
            { PlayerPos.FB, 0.85f },
            { PlayerPos.DM, 0.85f }
        };

        PosMMRx[PlayerPos.DM] = new Dictionary<PlayerPos, float>()
        {
            { PlayerPos.CB, 0.85f },
            { PlayerPos.AM, 0.85f }
        };

        PosMMRx[PlayerPos.LM] = new Dictionary<PlayerPos, float>()
        {
            { PlayerPos.AM, 0.8f },
            { PlayerPos.WG, 0.8f },
            { PlayerPos.FB, 0.8f }
        };

        PosMMRx[PlayerPos.AM] = new Dictionary<PlayerPos, float>()
        {
            { PlayerPos.DM, 0.85f },
            { PlayerPos.LM, 0.85f }
        };

        PosMMRx[PlayerPos.WG] = new Dictionary<PlayerPos, float>()
        {
            { PlayerPos.LM, 0.9f },
            { PlayerPos.ST, 0.8f }
        };

        PosMMRx[PlayerPos.ST] = new Dictionary<PlayerPos, float>()
        {
            { PlayerPos.AM, 0.85f },
            { PlayerPos.WG, 0.8f }
        };
    }

    public int GetMMRInPos(PlayerPos playedPos)
    {
        var mmr = MMR + 1_000_000;

        if (Pos == playedPos)
        {
            return mmr;
        }

        if (PosMMRx[Pos].ContainsKey(playedPos))
        {
            return (int)(mmr * PosMMRx[Pos][playedPos]);
        }
        else
        {
            return (int)(mmr * 0.25f);
        }
    }

    public float GetPosMMRx(PlayerPos myPos, PlayerPos playedPos)
    {
        if (myPos == playedPos)
        {
            return 1f;
        }

        if (PosMMRx[myPos].ContainsKey(playedPos))
        {
            return PosMMRx[myPos][playedPos];
        }
        else
        {
            return 0f;
        }
    }

    public static Dictionary<PlayerPos, string> PosColor = new Dictionary<PlayerPos, string>()
    {
        { PlayerPos.GK, "a79b77" },

        { PlayerPos.FB, "56a6cf" },
        { PlayerPos.CB, "56a6cf" },

        { PlayerPos.DM, "4c7e4d" },
        { PlayerPos.LM, "4c7e4d" },
        { PlayerPos.AM, "4c7e4d" },

        { PlayerPos.WG, "ff8f77" },
        { PlayerPos.ST, "ff8f77" }
    };

    public string GetPosColor()
    {
        return PosColor[Pos];
    }

    [RegularExpression("\\^[a-z][a-z0-9]*$\\")]
    public string Id { get; }
    public string Email { get; }
    public int MMR { get; set; }
    public int MMRIncrement { get; set; }
    public PlayerPos Pos { get; }

    public string Username => Email.Split("@")[0];

    public float Skill { get; }

    public int Games { get; set; }
    public int W { get; set; }
    public int D { get; set; }
    public int L { get; set; }

    public Dictionary<PlayerPos, int> PosMMR { get; private set; }

    static Dictionary<PlayerPos, Dictionary<PlayerPos, float>> Proficiency;

    public Player(string id, string email, int mMR, PlayerPos pos)
    {
        Id = id;
        Email = email;
        MMR = mMR;
        Pos = pos;
    }
}
