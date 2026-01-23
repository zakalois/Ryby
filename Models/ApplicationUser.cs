using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Ryby.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public string? ProfileImagePath { get; set; }
    }
}