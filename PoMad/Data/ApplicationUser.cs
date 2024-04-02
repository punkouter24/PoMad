using Microsoft.AspNetCore.Identity;

namespace PoMad.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public int IdealWeight { get; set; }
        public int Height { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
    }

}
