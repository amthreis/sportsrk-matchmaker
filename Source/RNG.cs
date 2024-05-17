namespace SRkMatchmakerAPI.Framework;

public static class RNG
{
    static Random rng;

    static RNG()
    {
        rng = new Random();
    }

    public static int RandiRange(int from, int to)
    {
        return rng.Next(from, to + 1);
    }
}
