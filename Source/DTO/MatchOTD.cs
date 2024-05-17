
namespace SRkMatchmakerAPI.Framework.DTO;

public struct MatchOTD
{
    public List<string> Home = new List<string>();
    public List<string> Away = new List<string>();

    public MatchOTD(Match match)
    {
        foreach (var p in match.Home)
        {
            Home.Add(p.Id);
        }
        foreach (var p in match.Away)
        {
            Away.Add(p.Id);
        }

        foreach (var p in Home)
        {
            Console.WriteLine(p);
        }
    }
}
