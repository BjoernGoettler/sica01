using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Services;

public class JwtTokenService
{
    private const string SecurityKey = "a8f5a7d9d3b1763b8b25e9f5f9c2e725b83e6d1a5f716a2b5c8f754d9e793d36";

    public AuthenticationToken CreateToken()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var claims = new List<Claim>
        {
            new Claim("scope", "tweet.write"),
            new Claim("scope", "tweet.read"),
            new Claim("scope", "user.read")
        };

        var tokenOptions = new JwtSecurityToken(
            signingCredentials: signingCredentials,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30));
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        var authToken = new AuthenticationToken
        {
            Value = tokenString
        };
        
        return authToken;
    }
}