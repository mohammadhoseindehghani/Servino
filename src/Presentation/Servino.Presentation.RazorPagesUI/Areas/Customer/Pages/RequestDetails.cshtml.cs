using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core.RequestAgg.Contracts.AppService;
using Servino.Domain.Core.RequestAgg.Dtos;
using Servino.Domain.Core.SuggestionAgg.Contracts.AppService;
using Servino.Domain.Core.SuggestionAgg.Dtos;
using Servino.Presentation.RazorPagesUI.Extensions;

namespace Servino.Presentation.RazorPagesUI.Areas.Customer.Pages
{
    [Authorize(Roles = "Customer")]
    public class RequestDetailsModel(
        IRequestAppService requestAppService,
        ISuggestionAppService suggestionAppService) : PageModel
    {
        public RequestDetailDto RequestDetail { get; set; } = new();
        public List<SuggestionSummaryDto> Suggestions { get; set; } = [];

        public string? MessageText { get; private set; }
        public string? MessageType { get; private set; } 

        public async Task<IActionResult> OnGetAsync(int id, string? msg, string? text, CancellationToken ct)
        {
            if (!string.IsNullOrEmpty(msg) && !string.IsNullOrEmpty(text))
            {
                MessageType = msg;
                MessageText = text;
            }

            var customerId = User.GetCustomerId();
            if (customerId == 0) return RedirectToPage("/Auth/Login/Index");

            var reqResult = await requestAppService.GetDetailsByIdAsync(id, ct);
            if (!reqResult.IsSuccess)
            {
                return RedirectToPage("./MyRequests", new { msg = "danger", text = reqResult.Message ?? "درخواست یافت نشد." });
            }

            if (reqResult.Data.CustomerId != customerId)
            {
                return RedirectToPage("./MyRequests", new { msg = "danger", text = "شما اجازه دسترسی به این درخواست را ندارید." });
            }

            RequestDetail = reqResult.Data;

            Suggestions = await suggestionAppService.GetByRequestIdAsync(id, ct);

            return Page();
        }

        public async Task<IActionResult> OnPostSelectExpertAsync(int requestId, int suggestionId, CancellationToken ct)
        {
            var customerId = User.GetCustomerId();
            if (customerId == 0) return Unauthorized();

            var result = await requestAppService.SelectExpertAsync(requestId, suggestionId, customerId, ct);

            return RedirectToPage(new
            {
                id = requestId,
                msg = result.IsSuccess ? "success" : "danger",
                text = result.Message
            });
        }

        public async Task<IActionResult> OnPostFinishWorkAsync(int requestId, CancellationToken ct)
        {
            var customerId = User.GetCustomerId();
            if (customerId == 0) return Unauthorized();

            var result = await requestAppService.MarkAsDoneAndPayAsync(requestId, customerId, ct);

            return RedirectToPage(new
            {
                id = requestId,
                msg = result.IsSuccess ? "success" : "danger",
                text = result.Message
            });
        }
    }
}
