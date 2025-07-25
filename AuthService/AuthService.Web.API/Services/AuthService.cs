using AuthService.Web.API.DTO;
using AuthService.Web.API.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Web.API.Services;

public class AuthService : IAuthService
{
    public Task<AuthResponseDTO> Authenticate(AuthRequestDTO request)
    {
        // Aqui você pode consultar o banco ou outro serviço para validar login/senha
        if (request.Login != "admin" || request.Password != "admin")
            throw new UnauthorizedAccessException("Credenciais inválidas");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes("l1SG<3v54n6");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, request.Login),
                new Claim("CustomClaim", "value")
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        AuthResponseDTO responseDTO = new AuthResponseDTO
        {
            Token = tokenHandler.WriteToken(token),
            Expiration = token.ValidTo
        };

        return Task.FromResult(responseDTO);
    }
}
