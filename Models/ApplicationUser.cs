using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Ryby_a_ulovky.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        public string? ProfileImagePath { get; set; }
    }
}