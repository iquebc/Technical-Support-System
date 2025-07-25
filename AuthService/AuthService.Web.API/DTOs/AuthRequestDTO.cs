using System.ComponentModel.DataAnnotations;

namespace AuthService.Web.API.DTO;

public class AuthRequestDTO
{
    
    public required string Login { get; set; }

    public required string Password { get; set; }
}
