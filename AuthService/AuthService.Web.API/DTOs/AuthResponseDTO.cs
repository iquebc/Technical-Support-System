namespace AuthService.Web.API.DTO;

public class AuthResponseDTO
{
    public required string Token { get; set; }

    public DateTime Expiration { get; set; }
}