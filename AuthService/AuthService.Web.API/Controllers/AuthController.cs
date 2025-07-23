using AuthService.Web.API.Model;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Web.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpPost]
    public ActionResult Login([FromBody]Auth auth)
    {
        return Ok(new { auth.Login, auth.Password });
    }
}

