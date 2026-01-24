using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Ryby.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        // Telefon NEpřidáváme, IdentityUser už má PhoneNumber

        public string? ProfileImagePath { get; set; }
    }
}