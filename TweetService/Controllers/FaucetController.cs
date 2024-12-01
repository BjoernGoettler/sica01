using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace TweetService.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class FaucetController: ControllerBase
{
    private readonly string _jwtKey;
    private readonly string _jwtIssuer;
    
    public FaucetController(string jwtKey, string jwtIssuer)
    {
        _jwtKey = jwtKey;
        _jwtIssuer = jwtIssuer;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

        var securityToken = new JwtSecurityToken(
            _jwtIssuer,
            _jwtIssuer,
            null,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
        );
        
        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return Ok(token);
    }

}