using System.ComponentModel.DataAnnotations;

namespace SRkMatchmakerAPI.Framework.DTO;

public struct UserDTO
{
    public string Id { get; set; }
    public string Email { get; set; }

    public UserDTO(string userId, string email)
    {
        Email = email;
        Id = userId;


        //Role = role;
    }
}
