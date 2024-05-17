using System.Collections.Generic;

namespace SRkMatchmakerAPI.Framework.DTO;

public struct MatchDTO
{
    public List<string> Home { get; set; } = new List<string>();
    public List<string> Away { get; set; } = new List<string>();

    public MatchDTO(Match match)
    {
        foreach (var p in match.Home)
        {
            Home.Add(p.Id);
        }
        foreach (var p in match.Away)
        {
            Away.Add(p.Id);
        }
    }
}
