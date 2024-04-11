using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PoMad.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Range(50, 500, ErrorMessage = "Weight must be between 50 and 500 lbs.")]
        public int IdealWeight { get; set; }

        [Range(50, 100, ErrorMessage = "Height must be between 50 and 100 inches.")]
        public int Height { get; set; }

        [Range(10, 100, ErrorMessage = "Age must be between 10 and 100.")]
        public int Age { get; set; }

        [Range(typeof(DateTime), "1/1/1900", "12/31/9999", ErrorMessage = "Birthday must be after January 1, 1900.")]
        public DateTime Birthday { get; set; }
    }

}
