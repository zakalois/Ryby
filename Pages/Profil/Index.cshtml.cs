using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ryby.Models;

namespace Ryby.Pages.Profil
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // 🔥 Tady je správný název – ne "User"
        public ApplicationUser? CurrentUser { get; set; }

        public string? AktualitaText { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                Console.WriteLine("OnGetAsync START");

                CurrentUser = await _userManager.GetUserAsync(User);

                Console.WriteLine("CurrentUser: " + (CurrentUser?.Email ?? "NULL"));

                AktualitaText = "Vítej v systému Ryby. Zde uvidíš nejnovější informace a zprávy.";
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION in OnGetAsync: " + ex.Message);
                throw;
            }
        }

    }
}