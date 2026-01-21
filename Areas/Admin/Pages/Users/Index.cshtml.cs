using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ryby_a_ulovky.Models;

namespace Ryby_a_ulovky.Areas.Admin.Pages.Users
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        public UserManager<ApplicationUser> UserManager { get; }
        public List<ApplicationUser> Users { get; set; } = new();

        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public void OnGet()
        {
            Users = UserManager.Users.ToList();
        }
    }
}