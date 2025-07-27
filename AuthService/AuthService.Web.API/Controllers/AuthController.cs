
using AuthService.Web.API.DTO;
using AuthService.Web.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Web.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{

    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpPost]
    public async Task<ActionResult> Login([FromBody] AuthRequestDTO auth)
    {
        AuthResponseDTO response = await _authService.Authenticate(auth);
        return Ok(response);
    }
}

