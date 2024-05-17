using SRkMatchmakerAPI.Framework.DTO;

namespace SRkMatchmakerAPI.Framework;

public struct MyResponse
{
    public List<MatchDTO> Matches { get; set; }
    public List<string> UnmatchedIds { get; set; }

    public MyResponse(List<MatchDTO> matches, List<string> unmatchedIds)
    {
        Matches = matches;
        UnmatchedIds = unmatchedIds;
    }
}
