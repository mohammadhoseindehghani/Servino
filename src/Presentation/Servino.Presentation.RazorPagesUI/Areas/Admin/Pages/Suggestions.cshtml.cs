using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Servino.Presentation.RazorPagesUI.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class SuggestionsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
