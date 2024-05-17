using System.ComponentModel.DataAnnotations;

namespace SRkMatchmakerAPI.Framework.DTO;

public struct UserDTO
{
    [MinLength(24)]
    [MaxLength(24)]
    [RegularExpression(@"[a-z][a-z0-9]*", ErrorMessage = "Not a valid Cuid2!")]
    public string Id { get; set; }
    public string Email { get; set; }

    public UserDTO(string userId, string email)
    {
        Email = email;
        Id = userId;


        //Role = role;
    }
}
