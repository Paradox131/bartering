using Microsoft.AspNetCore.Identity;

namespace bartering.Models
{
    public class User : IdentityUser
    {
       public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateOnly DateOfBirth { get; set; }


    }
}
