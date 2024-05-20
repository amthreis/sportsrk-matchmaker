using SRkMatchmakerAPI.Framework.DTO;

namespace SRkMatchmakerAPI.Framework.Mappers;

public static class PlayerMapper
{
    public static PlayerDTO ToPlayerDTO(this Player p)
    {
        return new PlayerDTO
        {
            User = new UserDTO
            {
                Email = p.Email,
                Id = p.Id
            },
            MMR = p.MMR,
            Pos = p.Pos
        };
    }
}