using System.Security.Claims;
using Application.Dtos;
using Identity.Dtos;
using Identity.Interfaces;
using Microsoft.AspNetCore.Identity;
using Identity.Models;
using Identity.Data.Dbcontext;
namespace Identity.Features
{
    public class UserAuthRepository : IUserAuthRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AuthUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenGeneration _tokenGeneration;

        public UserAuthRepository(AppDbContext context, UserManager<AuthUser> userManager, RoleManager<IdentityRole> roleManager, ITokenGeneration tokenGeneration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenGeneration = tokenGeneration;
        }

        public async Task<UserResponceDto> UserLogin(LoginUserDto user)
        {
            var authUser = await _userManager.FindByNameAsync(user.Username);
            UserResponceDto userResponceDto = new UserResponceDto();
            var isPasswordValid = await _userManager.CheckPasswordAsync(authUser, user.Password);
            if (!isPasswordValid)
            {
                return userResponceDto;
            }

            userResponceDto.Id = authUser.Id;
            userResponceDto.Username = authUser.name;
            userResponceDto.Role = await GetUserRolesAsync(authUser.name);
            userResponceDto.LoginStatus = true;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
            };
            foreach (var role in userResponceDto.Role)
            {
                claims.Add(new Claim(ClaimTypes.Role , role));
            }

            userResponceDto.Token = _tokenGeneration.GenerateToken(claims);
            
            return userResponceDto;

        }

        public async Task<IList<string>> GetUserRolesAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }
        
        public async Task<UserResponceDto> GetUser(string username)
        {
            var responce = new UserResponceDto();
            var getUser = await _userManager.FindByNameAsync(username);
            var roles = await _userManager.GetRolesAsync(getUser);
            if (getUser != null)
            {
                responce.Username = getUser.name;
                responce.Id = getUser.Id;
                responce.Role = roles;
            }
            return responce;
        }
        public async Task<UserResponceDto> AddUserAsync(AuthUserDto user, string role)
        {
            var _user = new AuthUser();
            _user.name = user.name;
            _user.UserName = user.name;
            _user.Email = "hello@gmail.com";
            var result = await _userManager.CreateAsync(_user, user.Password);
            if (!result.Succeeded)
            {
                throw new Exception("An error occurred while creating the user.");
            }
            await _userManager.AddToRoleAsync(_user, role);
            var createdUser = await _userManager.FindByNameAsync(_user.UserName);
            var responce = new UserResponceDto
            {
                Username = createdUser.name,
                Id = createdUser.Id,
                //Role = role,
            };
            return responce;
        }

    }
}
