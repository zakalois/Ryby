using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ryby.Models;

namespace Ryby.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _environment;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public string? ProfilePicturePath { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string? FirstName { get; set; }
            public string? LastName { get; set; }

            public string? PhoneNumber { get; set; }
            public string? Email { get; set; }

            public string? FishingLicenseNumber { get; set; }
            public string? TroutPermitNumber { get; set; }
            public string? NonTroutPermitNumber { get; set; }

            public string? ProfilePicturePath { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Username = user.UserName;

            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,

                PhoneNumber = user.PhoneNumber ?? "",
                Email = user.Email ?? "",

                FishingLicenseNumber = user.FishingLicenseNumber,
                TroutPermitNumber = user.TroutPermitNumber,
                NonTroutPermitNumber = user.NonTroutPermitNumber,

                ProfilePicturePath = user.ProfilePicturePath
            };

            ProfilePicturePath = user.ProfilePicturePath;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? profileImage)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User not found.");

            if (!ModelState.IsValid)
                return Page();

            // Uložení textových údajù
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;

            user.PhoneNumber = Input.PhoneNumber;
            user.Email = Input.Email;

            user.FishingLicenseNumber = Input.FishingLicenseNumber;
            user.TroutPermitNumber = Input.TroutPermitNumber;
            user.NonTroutPermitNumber = Input.NonTroutPermitNumber;

            // Uložení profilového obrázku
            if (profileImage != null && profileImage.Length > 0)
            {
                // Limit 2 MB
                if (profileImage.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError(string.Empty, "Soubor je pøíliš velký. Maximální velikost je 2 MB.");
                    return Page();
                }

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "profile");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{user.Id}{Path.GetExtension(profileImage.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profileImage.CopyToAsync(stream);
                }

                user.ProfilePicturePath = fileName;
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            StatusMessage = "Profil byl úspìšnì aktualizován";
            return RedirectToPage();
        }
    }
}