namespace SRkMatchmakerAPI.Framework.DTO;

public struct MakeGamesDTO
{
    public PlayerDTO[] Players;

    public MakeGamesDTO(PlayerDTO[] players)
    {
        Players = players;
    }
}

