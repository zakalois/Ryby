using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ryby.Pages
{
    [Authorize(Roles = "Admin")]
    public class TestAdminModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}