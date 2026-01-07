using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core.RequestAgg.Contracts.AppService;
using Servino.Domain.Core.RequestAgg.Dtos;
using Servino.Presentation.RazorPagesUI.Extensions;

namespace Servino.Presentation.RazorPagesUI.Areas.Expert.Pages
{
    [Authorize(Roles = "Expert")]
    public class AvailableRequestsModel(IRequestAppService requestAppService) : PageModel
    {
        public List<RequestSummaryDto> Requests { get; set; } = [];

        public async Task OnGet(CancellationToken ct)
        {
            var userId = User.GetUserId();
            if (userId > 0)
            {
                Requests = await requestAppService.GetAvailableForExpertAsync(userId, ct);
            }
        }
    }
}
