using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ryby_a_ulovky.Pages
{
    [Authorize(Roles = "Admin")]
    public class TestAdminModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}