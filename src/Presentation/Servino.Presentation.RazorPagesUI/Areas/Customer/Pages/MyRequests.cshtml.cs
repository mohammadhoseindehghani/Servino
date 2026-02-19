using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core.RequestAgg.Contracts.AppService;
using Servino.Domain.Core.RequestAgg.Dtos;
using Servino.Presentation.RazorPagesUI.Extensions;

namespace Servino.Presentation.RazorPagesUI.Areas.Customer.Pages
{
    [Authorize(Roles = "Customer")]
    public class MyRequestsModel(IRequestAppService requestAppService) : PageModel
    {
        public List<RequestSummaryDto> Requests { get; set; } = [];

        public async Task OnGet(CancellationToken ct)
        {
            var customerId = User.GetCustomerId();
            if (customerId > 0)
            {
                Requests = await requestAppService.GetByCustomerIdAsync(customerId, ct);
            }
        }
    }
}
