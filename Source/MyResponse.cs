using SRkMatchmakerAPI.Framework.DTO;

namespace SRkMatchmakerAPI.Framework;

public struct MyResponse
{
    public List<MatchOTD> Matches { get; set; }
    public List<string> UnmatchedIds { get; set; }

    public MyResponse(List<MatchOTD> matches, List<string> unmatchedIds)
    {
        Matches = matches;
        UnmatchedIds = unmatchedIds;
    }
}
