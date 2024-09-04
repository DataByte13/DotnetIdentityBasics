using Microsoft.EntityFrameworkCore;
namespace Identity.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
