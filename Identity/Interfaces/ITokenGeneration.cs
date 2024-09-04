using System.Security.Claims;
using Identity.Dtos;

namespace Identity.Interfaces;

public interface ITokenGeneration
{
    string GenerateToken(List<Claim> claims);
    

}