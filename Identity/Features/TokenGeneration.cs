using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Identity.Dtos;
using Identity.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
namespace Identity.Features;
public class TokenGeneration : ITokenGeneration
{
    private readonly IConfiguration _configuration;

    public TokenGeneration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(List<Claim> claims)
    {
        var jwtOptions = _configuration.GetSection("JwtOptions");

        var issuer = jwtOptions.GetValue<string>("Issuer");
        var audience = jwtOptions.GetValue<string>("Audience");
        var secretKey = jwtOptions.GetValue<string>("SigningKey");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            // {
            //     new Claim(ClaimTypes.Name, userId),
            //     new Claim(ClaimTypes.Role, role)
            // }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        // ResponceTokenDto responceToken = new ResponceTokenDto
        // {
        //     Token = tokenHandler.WriteToken(token)
        // };
        return tokenHandler.WriteToken(token);
    }

    public ResponceTokenDto GenerateToken(string userId, string role)
    {
        throw new NotImplementedException();
    }
}