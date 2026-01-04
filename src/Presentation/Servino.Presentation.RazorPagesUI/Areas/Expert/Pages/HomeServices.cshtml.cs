using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core._common;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Presentation.RazorPagesUI.Areas.Expert.Pages
{
    [Authorize(Roles = "Expert")]
    public class HomeServicesModel(
            IExpertAppService expertAppService,
            IUserAppService userAppService) : PageModel
    {
        public List<ExpertServiceItemDto> ServiceItems { get; set; } = [];

        [BindProperty]
        public List<int> SelectedServiceIds { get; set; } = [];

        [TempData] public string? SuccessMessage { get; set; }
        [TempData] public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGet(CancellationToken ct)
        {
            var userId = await GetCurrentUserIdAsync(ct);
            if (userId == 0) return RedirectToPage("/Auth/Login/Index");

            var servicesResult = await expertAppService.GetServicesForEditAsync(userId, ct);
            if (servicesResult.IsSuccess)
            {
                ServiceItems = servicesResult.Data;
            }
            else
            {
                ErrorMessage = "خطا در دریافت لیست خدمات.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken ct)
        {
            var userId = await GetCurrentUserIdAsync(ct);
            if (userId == 0) return RedirectToPage("/Auth/Login/Index");

            var result = await expertAppService.UpdateServicesAsync(userId, SelectedServiceIds, ct);

            if (result.IsSuccess)
            {
                SuccessMessage = "لیست خدمات با موفقیت ذخیره شد.";
                var servicesResult = await expertAppService.GetServicesForEditAsync(userId, ct);
                if (servicesResult.IsSuccess) ServiceItems = servicesResult.Data;

                return Page();
            }

            ErrorMessage = result.Message;
            return Page();
        }

        private async Task<int> GetCurrentUserIdAsync(CancellationToken ct)
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName)) return 0;
            var search = new PaginationRequestDto { SearchKey = userName };
            var listResult = await userAppService.GetUsersListAsync(search, ct);
            return listResult.IsSuccess && listResult.Data.Any() ? listResult.Data.First().Id : 0;
        }
    }
}
