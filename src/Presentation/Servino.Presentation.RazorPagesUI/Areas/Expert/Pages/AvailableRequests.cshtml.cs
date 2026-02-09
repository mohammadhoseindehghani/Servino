using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core.RequestAgg.Contracts.AppService;
using Servino.Domain.Core.RequestAgg.Dtos;
using Servino.Domain.Core.SuggestionAgg.Contracts.AppService;
using Servino.Presentation.RazorPagesUI.Extensions;

namespace Servino.Presentation.RazorPagesUI.Areas.Expert.Pages
{
    [Authorize(Roles = "Expert")]
    public class AvailableRequestsModel(IRequestAppService requestAppService, ISuggestionAppService suggestionAppService) : PageModel
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

        public async Task<IActionResult> OnGetSuggestionDetailsAsync(int id, CancellationToken ct)
        {
            var suggestion = await suggestionAppService.GetByIdAsync(id, ct);
            if (suggestion == null!)
                return new JsonResult(null);

            return new JsonResult(new
            {
                price = suggestion.Data.SuggestedPrice,
                duration = suggestion.Data.EstimatedDurationHours,
                description = suggestion.Data.Note,
                createdAt = suggestion.Data.CreatedAt.ToString("yyyy/MM/dd")
            });
        }


    }
}
