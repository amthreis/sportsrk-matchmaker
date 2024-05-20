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

    public static void SetSeed(int seed)
    {
        rng = new Random(seed);
    }
}
