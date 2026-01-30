using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ryby.Pages.Ulovky
{
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}