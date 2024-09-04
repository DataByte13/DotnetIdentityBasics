using Application.Dtos;
using Identity.Dtos;
namespace Identity.Interfaces
{
    public interface IUserAuthRepository
    {
        Task<UserResponceDto> AddUserAsync(AuthUserDto user, string role);
        Task<UserResponceDto> UserLogin(LoginUserDto user);
    }
}


