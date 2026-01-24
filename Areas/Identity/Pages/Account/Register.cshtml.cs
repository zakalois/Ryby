using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Ryby.Models;
using System.IO;

namespace Ryby.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _env = env;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Jméno")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Příjmení")]
            public string LastName { get; set; }

            [Required]
            [Phone]
            [Display(Name = "Telefon")]
            public string Phone { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Heslo musí mít alespoň {2} znaků.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Heslo")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potvrzení hesla")]
            [Compare("Password", ErrorMessage = "Hesla se neshodují.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Profilová fotka")]
            public IFormFile? ProfileImage { get; set; }
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    PhoneNumber = Input.Phone
                };

                // Uložení profilové fotky
                if (Input.ProfileImage != null)
                {
                    const long maxSize = 2 * 1024 * 1024; // 2 MB

                    if (Input.ProfileImage.Length > maxSize)
                    {
                        ModelState.AddModelError(string.Empty, "Soubor je příliš velký. Maximální velikost je 2 MB.");
                        return Page();
                    }

                    var folder = Path.Combine(_env.WebRootPath, "images/profile");
                    Directory.CreateDirectory(folder);

                    var extension = Path.GetExtension(Input.ProfileImage.FileName);
                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Input.ProfileImage.CopyToAsync(stream);
                    }

                    user.ProfileImagePath = fileName;
                }

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Potvrzení emailu",
                        $"Potvrďte prosím svůj účet kliknutím <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>zde</a>.");

                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}