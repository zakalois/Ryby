using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ryby.Models;

namespace Ryby.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public IList<AuthenticationScheme> ExternalLogins { get; set; } = new List<AuthenticationScheme>();

        public string? ReturnUrl { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string? Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string? Password { get; set; }

            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
                ModelState.AddModelError(string.Empty, ErrorMessage);

            ReturnUrl = returnUrl ?? Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            try
            {
                Console.WriteLine(">>> OnPostAsync START <<<");

                ReturnUrl = returnUrl ?? Url.Content("~/");

                if (!ModelState.IsValid)
                {
                    Console.WriteLine("ModelState is invalid.");
                    return Page();
                }

                Console.WriteLine("Attempting login for: " + Input.Email);

                var result = await _signInManager.PasswordSignInAsync(
                    Input.Email!,
                    Input.Password!,
                    Input.RememberMe,
                    lockoutOnFailure: false);

                Console.WriteLine("Login result: " + result.Succeeded);

                if (result.Succeeded)
                {
                    Console.WriteLine("Login succeeded for: " + Input.Email);
                    return LocalRedirect("/Profil/Index");
                }

                Console.WriteLine("Login failed for: " + Input.Email);
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine(">>> EXCEPTION CAUGHT <<<");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}