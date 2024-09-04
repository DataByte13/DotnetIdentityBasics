using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Identity.Models;

namespace Identity.Data.Role
{
    public class AdminSeeder
    {
        private readonly UserManager<AuthUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminSeeder(UserManager<AuthUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAdminAsync()
        {
            var userName = "admin";
            var Password = "admin";
            if (await _userManager.FindByNameAsync(userName) == null)
            {
                var admin = new AuthUser
                {
                    UserName = userName,
                    name = userName,
                };
                var result = await _userManager.CreateAsync(admin, Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create admin user: {errors}");
                }
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "Admin");
                }
                else
                {
                    // Handle failure case
                    throw new Exception("Failed to create admin user");
                }

            }
        }
    }
}

