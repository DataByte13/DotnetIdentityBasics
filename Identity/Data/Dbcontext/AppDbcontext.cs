using Microsoft.EntityFrameworkCore;
using Identity.Models;

namespace Identity.Data.Dbcontext
{
    public class AppDbContext : DbContext
    {
        //public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<AppUser> AppUsers { get; set; }

    }

}
