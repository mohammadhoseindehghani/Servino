using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core.RequestAgg.Contracts.AppService;
using Servino.Domain.Core.RequestAgg.Dtos;

namespace Servino.Presentation.RazorPagesUI.Areas.Expert.Pages
{
    [Authorize(Roles = "Expert")]
    public class RequestDetailsModel(IRequestAppService requestAppService) : PageModel
    {
        public RequestDetailDto Request { get; set; } = default!;

        public async Task<IActionResult> OnGet(int id, CancellationToken ct)
        {
            var result = await requestAppService.GetDetailsByIdAsync(id, ct);

            if (!result.IsSuccess)
                return NotFound();

            Request = result.Data;
            return Page();
        }
    }
}
