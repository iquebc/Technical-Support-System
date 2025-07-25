using AuthService.Web.API.DTO;

namespace AuthService.Web.API.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDTO> Authenticate(AuthRequestDTO request);
}