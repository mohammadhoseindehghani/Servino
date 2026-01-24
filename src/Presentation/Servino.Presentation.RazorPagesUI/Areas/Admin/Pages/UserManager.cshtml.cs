using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core._common;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Presentation.RazorPagesUI.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class UserManagerModel(
            IUserAppService userAppService,
            IExpertAppService expertAppService 
            ) : PageModel
    {
        public List<UserSummaryDto> Users { get; set; } = [];

        [BindProperty(SupportsGet = true)]
        public string? SearchKey { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        [BindProperty]
        public AdminUpdateUserDto EditModel { get; set; } = new();

        [BindProperty]
        public CreateUserByAdminDto CreateModel { get; set; } = new();

        [BindProperty]
        public ChargeBalanceModel ChargeModel { get; set; } = new();

        [BindProperty]
        public List<int> SelectedServiceIds { get; set; } = [];

        [BindProperty]
        public int TargetExpertId { get; set; } 

        [TempData] public string? SuccessMessage { get; set; }
        [TempData] public string? ErrorMessage { get; set; }

        public async Task OnGet(CancellationToken ct)
        {
            var pagination = new PaginationRequestDto
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
                SearchKey = SearchKey
            };
            var result = await userAppService.GetUsersListAsync(pagination, ct);
            if (result.IsSuccess) Users = result.Data ?? [];
            else ErrorMessage = result.Message;
        }

        public async Task<IActionResult> OnPostCreateAsync(CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(CreateModel.FirstName) ||
                string.IsNullOrWhiteSpace(CreateModel.LastName) ||
                string.IsNullOrWhiteSpace(CreateModel.Email) ||
                string.IsNullOrWhiteSpace(CreateModel.Mobile) ||
                string.IsNullOrWhiteSpace(CreateModel.Password) ||
                string.IsNullOrWhiteSpace(CreateModel.Role))
            {
                ErrorMessage = "تمامی فیلدها الزامی هستند.";
                return RedirectToPage();
            }

            var result = await userAppService.CreateUserByAdminAsync(CreateModel, ct);

            if (result.IsSuccess) SuccessMessage = result.Message;
            else ErrorMessage = result.Message;

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync(CancellationToken ct)
        {
            var result = await userAppService.AdminUpdateUserAsync(EditModel, ct);

            if (result.IsSuccess) SuccessMessage = result.Message;
            else ErrorMessage = result.Message;

            return RedirectToPage(new { PageNumber, SearchKey });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, CancellationToken ct)
        {
            var result = await userAppService.DeleteUserAsync(id, ct);
            if (result.IsSuccess) SuccessMessage = result.Message;
            else ErrorMessage = result.Message;
            return RedirectToPage(new { PageNumber, SearchKey });
        }

        public async Task<IActionResult> OnPostChargeAsync(CancellationToken ct)
        {
            if (ChargeModel.UserId == 0 || ChargeModel.Amount == 0) return RedirectToPage();
            var result = await userAppService.ChangeUserBalanceAsync(ChargeModel.UserId, ChargeModel.Amount, ct);
            if (result.IsSuccess) SuccessMessage = result.Message;
            else ErrorMessage = result.Message;
            return RedirectToPage(new { PageNumber, SearchKey });
        }

        public async Task<JsonResult> OnGetExpertServicesAsync(int userId, CancellationToken ct)
        {
            var result = await expertAppService.GetServicesForEditAsync(userId, ct);
            if (!result.IsSuccess) return new JsonResult(new List<ExpertServiceItemDto>());

            return new JsonResult(result.Data);
        }

        public async Task<IActionResult> OnPostUpdateServicesAsync(CancellationToken ct)
        {
            if (TargetExpertId == 0) return RedirectToPage();

            var result = await expertAppService.UpdateServicesAsync(TargetExpertId, SelectedServiceIds, ct);

            if (result.IsSuccess) SuccessMessage = "خدمات متخصص با موفقیت بروزرسانی شد.";
            else ErrorMessage = result.Message;

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
