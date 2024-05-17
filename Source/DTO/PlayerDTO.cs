
namespace SRkMatchmakerAPI.Framework.DTO;

public enum PlayerDTOSide { Home, Away }

public struct PlayerDTO
{
    public UserDTO User { get; set; }
    public int MMR { get; set; }
    public PlayerPos Pos { get; set; }

    public PlayerDTO(string id, string name, int mmr, PlayerPos pos)
    {
        User = new UserDTO(id, name);

        MMR = mmr;
        Pos = pos;
    }
}
