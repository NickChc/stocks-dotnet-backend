using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Interfaces;
using Api.Models;
using Microsoft.IdentityModel.Tokens;

namespace Api.Service;

public class TokenService(IConfiguration config) : ITokenService
{
    private readonly IConfiguration _config = config;
    private readonly SymmetricSecurityKey _key = new(
        Encoding.UTF8.GetBytes(config["JWT:SigninKey"]!)
    );

    public string CreateToken(AppUser user)
    {
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.GivenName, user.UserName!),
        ];

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds,
            Issuer = _config["JWT:Issuer"],
            Audience = _config["JWT:Audience"],
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescription);

        return tokenHandler.WriteToken(token);
    }
}
