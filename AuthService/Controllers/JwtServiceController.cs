using AuthService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class JwtServiceController(JwtTokenService jwtTokenService): ControllerBase
{
    [HttpPost]
    public IActionResult Login()
        => Ok(jwtTokenService.CreateToken());
}