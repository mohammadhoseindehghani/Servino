using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core._common;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Presentation.RazorPagesUI.Areas.Admin.Pages
{
    public class UserManagerModel(IUserAppService userAppService) : PageModel
    {
        public List<UserSummaryDto> Users { get; set; } = [];

        [BindProperty(SupportsGet = true)]
        public string? SearchKey { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        [BindProperty]
        public ChargeBalanceModel ChargeModel { get; set; } = new();

        [TempData]
        public string? SuccessMessage { get; set; }
        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task OnGet(CancellationToken ct)
        {
            var pagination = new PaginationRequestDto
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
                SearchKey = SearchKey
            };

            var result = await userAppService.GetUsersListAsync(pagination, ct);

            if (result.IsSuccess)
            {
                Users = result.Data ?? [];
            }
            else
            {
                ErrorMessage = result.Message;
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, CancellationToken ct)
        {
            var result = await userAppService.DeleteUserAsync(id, ct);

            if (result.IsSuccess)
                SuccessMessage = result.Message;
            else
                ErrorMessage = result.Message;

            return RedirectToPage(new { PageNumber, SearchKey });
        }

        public async Task<IActionResult> OnPostChargeAsync(CancellationToken ct)
        {

            if (ChargeModel.UserId == 0 || ChargeModel.Amount == 0)
            {
                ErrorMessage = "اطلاعات وارد شده معتبر نیست.";
                return RedirectToPage(new { PageNumber, SearchKey });
            }

            var result = await userAppService.ChangeUserBalanceAsync(ChargeModel.UserId, ChargeModel.Amount, ct);

            if (result.IsSuccess)
                SuccessMessage = result.Message;
            else
                ErrorMessage = result.Message;

            return RedirectToPage(new { PageNumber, SearchKey });
        }

        public class ChargeBalanceModel
        {
            public int UserId { get; set; }
            public decimal Amount { get; set; }
            public string? Description { get; set; }
        }
    }
}
