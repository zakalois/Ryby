using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Ryby.Models;   // ← DŮLEŽITÉ!
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Ryby.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ForgotPasswordModel> _logger;

        public ForgotPasswordModel(
    UserManager<ApplicationUser> userManager,
    IEmailSender emailSender,
    ILogger<ForgotPasswordModel> logger)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "E-mail je povinný.")]
            [EmailAddress(ErrorMessage = "Neplatný formát e-mailu.")]
            public string Email { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.FindByEmailAsync(Input.Email);

            // Neprozrazujeme, zda uživatel existuje
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            // ⭐ VYGENEROVÁNÍ TOKENU
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // ⭐ VYTVOŘENÍ ODKAZU
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { area = "Identity", code = encodedCode },
                protocol: Request.Scheme);

            // ⭐ ODESLÁNÍ EMAILU
            await _emailSender.SendEmailAsync(
                Input.Email,
                "Reset hesla",
                $"Obnovte heslo kliknutím sem: <a href='{callbackUrl}'>obnovit heslo</a>");

            return RedirectToPage("./ForgotPasswordConfirmation");
        }
    }
}