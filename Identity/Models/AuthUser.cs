using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Identity.Models
{
    public class AuthUser : IdentityUser
    {
        public string name { get; set; }
    }
}
