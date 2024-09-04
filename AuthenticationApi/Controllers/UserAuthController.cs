using System.Security.Claims;
using Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Identity.Dtos;
using Identity.Interfaces;

namespace AuthenticationApi.Controllers;


[ApiController]
[Route("[controller]")]
public class UserAuthController : Controller
{
    private readonly IUserAuthRepository _userAuthRepository;
    private readonly ITokenGeneration _tokenGenerationService; //private readonly IHttpClientFactory _httpClientFactory;


    public UserAuthController(
    IUserAuthRepository userAuthRepository,
    ITokenGeneration tokenService)
    //IHttpClientFactory httpClientFactory)
    {
        _tokenGenerationService = tokenService;
        //_httpClientFactory = httpClientFactory;
        _userAuthRepository = userAuthRepository;
    }
    [HttpPost("AddUser")]
    [AllowAnonymous]
    public async Task<ActionResult<UserResponceDto>> AddUser(AuthUserDto request, string role)
    {
        if (request == null)
        {
            return BadRequest("Invalid user data.");
        }
    
        var response = await _userAuthRepository.AddUserAsync(request, role);
    
        return Ok(response);
        //var hashedPas sword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        // var user = new AppUser
        // {
        //     UserName = request.Username,
        //     Password = hashedPassword,
        // };
        // _userAuthRepository.AddUser(user );
        // var responseUser = _userAuthRepository.GetUser(user.UserName);
        // var response = new UserResponceDto
        // {
        //     Id = responseUser.Id,
        //     Username = responseUser.UserName,
        //     Password = responseUser.Password
        // };
    }

    [HttpGet("tokenTest")]
    [Authorize(Roles = "Admin")]
    public ActionResult<string> TokenTest()
    {
        return Ok("its work");
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<HttpResponse>> UserLogin(LoginUserDto request)
    {
        if (request == null)
        {
            return BadRequest("Invalid user data.");
        }
        
        var loginResult = await _userAuthRepository.UserLogin(request);
        
        if (!loginResult.LoginStatus)
        {
            return BadRequest("username or Password is wrong!! ");
        }

        return Ok(loginResult);
        // var requestData = new FormUrlEncodedContent(new[]
        // {
        //         new KeyValuePair<string, string>("grant_type", "password"),
        //         new KeyValuePair<string, string>("username", request.Username),
        //         new KeyValuePair<string, string>("password", request.Password)
        // });


        //        var content = await response.Content.ReadAsStringAsync();
        //var token = _tokenGenerationService.GenerateToken(request.Username, );
        // if (string.IsNullOrEmpty(token.Token))
        // {
        //     return BadRequest("Unable to generate token.");
        // }
        //  var user = await _userAuthRepository.GetUser(request.Username);
        //  List<Claim> claims = new List<Claim>{
        //    new Claim(ClaimTypes.Name , user.Username),
        //    new Claim(ClaimTypes.Role , "Admin")
        //  };
        // return Ok(new { Token = token });


    }
}

