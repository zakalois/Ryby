using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Ryby.Models;
using System.IO;

namespace Ryby.Pages.Profil
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public IndexModel(UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }

        public string? ProfileImagePath { get; set; }

        public async Task OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            ProfileImagePath = user?.ProfileImagePath;
        }

        public async Task<IActionResult> OnPostAsync(IFormFile ProfileImage)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ProfileImage != null)
            {
                var folder = Path.Combine(_env.WebRootPath, "images/profile");
                Directory.CreateDirectory(folder);

                // Smazání staré fotky, pokud existuje
                if (!string.IsNullOrEmpty(user.ProfileImagePath))
                {
                    var oldPath = Path.Combine(folder, user.ProfileImagePath);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                // Unikátní název nové fotky
                var extension = Path.GetExtension(ProfileImage.FileName);
                var fileName = $"{user.Id}_{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(folder, fileName);

                // Uložení nové fotky
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(stream);
                }

                // Uložení názvu do databáze
                user.ProfileImagePath = fileName;
                await _userManager.UpdateAsync(user);
            }

            return RedirectToPage();
        }
    }
}