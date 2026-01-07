using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core._common;
using Servino.Domain.Core.RequestAgg.Contracts.AppService;
using Servino.Domain.Core.RequestAgg.Dtos;

namespace Servino.Presentation.RazorPagesUI.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class RequestsModel(IRequestAppService requestAppService) : PageModel
    {
        public List<RequestSummaryDto> Requests { get; set; } = [];

        [BindProperty(SupportsGet = true)]
        public string SearchKey { get; set; }

        public async Task OnGet(CancellationToken ct)
        {
            var search = new PaginationRequestDto { SearchKey = SearchKey, PageSize = 50 };
            Requests = await requestAppService.GetAllAsync(search, null, null, ct);
        }
    }
}
