using System.ComponentModel.DataAnnotations;

namespace SRkMatchmakerAPI.Framework.DTO;

public class MakeGamesDTO
{
    //[MinLength(22)]
    public PlayerDTO[] Players { get; set; }
    
    //public bool Defer { get; set; }

    public MakeGamesDTO(PlayerDTO[] players)
    {
        Players = players;
    }
}

