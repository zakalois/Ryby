using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Ryby.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? FishingLicenseNumber { get; set; } // Rybářský lístek
        public string? TroutPermitNumber { get; set; }     // Povolenka pstruhová
        public string? NonTroutPermitNumber { get; set; }  // Povolenka mimopstruhová

        public string? ProfilePicturePath { get; set; }
    }
}