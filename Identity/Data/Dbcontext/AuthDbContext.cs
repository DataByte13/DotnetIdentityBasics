using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Identity.Models;
namespace Identity.Data.Dbcontext
{
    public class AuthDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<AuthUser>
    {

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }
    }
}
