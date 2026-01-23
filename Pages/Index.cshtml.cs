using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ryby.Data;
using Ryby.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Ryby.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(
            ApplicationDbContext context,
            ILogger<IndexModel> logger,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public Aktualita PosledniAktualita { get; set; }
        public ApplicationUser LoggedUser { get; set; }

        public async Task OnGetAsync()
        {
            // Naètení poslední aktuality
            PosledniAktualita = await _context.Aktuality
                .OrderByDescending(a => a.Datum)
                .FirstOrDefaultAsync();

            // Naètení pøihlášeného uživatele
            LoggedUser = await _userManager.GetUserAsync(User);
        }
    }
}