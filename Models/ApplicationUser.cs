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

        [Phone]
        public string? Phone { get; set; }

        // 🔥 Tohle budeme používat pro profilovou fotku
        public string? ProfileImagePath { get; set; }
    }
}