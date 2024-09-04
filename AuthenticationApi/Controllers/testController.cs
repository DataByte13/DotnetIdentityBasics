using System.Security.Claims;
using Identity.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace AuthenticationApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenGeneration _tokenGenerationService;

    public AuthController(ITokenGeneration tokenGenerationService)
    {
        _tokenGenerationService = tokenGenerationService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Validate user credentials (this is just an example)
        List<Claim> climes = new List<Claim>
        {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.Role, "Admin")
        };
        if (request.Username == "user" && request.Password == "user")
        {
            var token = _tokenGenerationService.GenerateToken(climes);
            return Ok(new { Token = token });
        }
        return Unauthorized();
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
