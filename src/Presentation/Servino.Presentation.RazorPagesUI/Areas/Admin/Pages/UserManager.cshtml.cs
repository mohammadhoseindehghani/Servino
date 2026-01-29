using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core._common;
using Servino.Domain.Core.LocationAgg.Contracts.AppService;
using Servino.Domain.Core.LocationAgg.Dtos;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Presentation.RazorPagesUI.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class UserManagerModel(IUserAppService userAppService, IExpertAppService expertAppService, IProvinceAppService provinceAppService,
        ICityAppService cityAppService)
        : PageModel
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
        public List<SelectListDto> Provinces { get; set; } = [];


        [BindProperty]
        public int TargetExpertId { get; set; }

        public string? MessageText { get; private set; }
        public string? MessageType { get; private set; } 

        public async Task OnGet(string? msg, string? text, CancellationToken ct)
        {
            if (!string.IsNullOrEmpty(msg) && !string.IsNullOrEmpty(text))
            {
                MessageType = msg;
                MessageText = text;
            }

            var pagination = new PaginationRequestDto
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
                SearchKey = SearchKey
            };
            Provinces = await provinceAppService.GetAllForDropdownAsync(ct);

            var result = await userAppService.GetUsersListAsync(pagination, ct);
            if (result.IsSuccess)
            {
                Users = result.Data ?? [];
            }
            else
            {
                MessageType = "danger";
                MessageText = result.Message;
            }
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
                return RedirectToPage(new { msg = "danger", text = "تمامی فیلدها الزامی هستند." });
            }

            var result = await userAppService.CreateUserByAdminAsync(CreateModel, ct);
            return RedirectToPage(new
            {
                msg = result.IsSuccess ? "success" : "danger",
                text = result.Message ?? (result.IsSuccess ? "کاربر با موفقیت ایجاد شد." : "خطایی رخ داد.")
            });
        }

        public async Task<IActionResult> OnPostEditAsync(CancellationToken ct)
        {
            var result = await userAppService.AdminUpdateUserAsync(EditModel, ct);
            return RedirectToPage(new
            {
                PageNumber,
                SearchKey,
                msg = result.IsSuccess ? "success" : "danger",
                text = result.Message ?? (result.IsSuccess ? "تغییرات با موفقیت ذخیره شد." : "خطایی رخ داد.")
            });
        }
        public async Task<JsonResult> OnGetCitiesByProvinceAsync(int provinceId, CancellationToken ct)
        {
            var cities = await cityAppService.GetCitiesByProvinceIdAsync(provinceId, ct);
            return new JsonResult(cities);
        }


        public async Task<IActionResult> OnPostDeleteAsync(int id, CancellationToken ct)
        {
            var result = await userAppService.DeleteUserAsync(id, ct);
            return RedirectToPage(new
            {
                PageNumber,
                SearchKey,
                msg = result.IsSuccess ? "success" : "danger",
                text = result.Message ?? (result.IsSuccess ? "کاربر با موفقیت حذف شد." : "خطایی رخ داد.")
            });
        }

        public async Task<IActionResult> OnPostChargeAsync(CancellationToken ct)
        {
            if (ChargeModel.UserId == 0 || ChargeModel.Amount <= 0)
            {
                return RedirectToPage(new
                {
                    PageNumber,
                    SearchKey,
                    msg = "danger",
                    text = "مبلغ یا کاربر معتبر نیست."
                });
            }

            var result = await userAppService.ChangeUserBalanceAsync(ChargeModel.UserId, ChargeModel.Amount, ct);
            return RedirectToPage(new
            {
                PageNumber,
                SearchKey,
                msg = result.IsSuccess ? "success" : "danger",
                text = result.Message ?? (result.IsSuccess ? "حساب با موفقیت شارژ شد." : "خطایی رخ داد.")
            });
        }

        public async Task<JsonResult> OnGetExpertServicesAsync(int userId, CancellationToken ct)
        {
            var result = await expertAppService.GetServicesForEditAsync(userId, ct);
            if (!result.IsSuccess) return new JsonResult(new List<ExpertServiceItemDto>());
            return new JsonResult(result.Data);
        }

        public async Task<IActionResult> OnPostUpdateServicesAsync(CancellationToken ct)
        {
            if (TargetExpertId == 0)
            {
                return RedirectToPage(new
                {
                    PageNumber,
                    SearchKey,
                    msg = "danger",
                    text = "متخصص معتبر نیست."
                });
            }

            var result = await expertAppService.UpdateServicesAsync(TargetExpertId, SelectedServiceIds, ct);
            return RedirectToPage(new
            {
                PageNumber,
                SearchKey,
                msg = result.IsSuccess ? "success" : "danger",
                text = result.IsSuccess ? "خدمات متخصص با موفقیت بروزرسانی شد." : result.Message
            });
        }

        public async Task<JsonResult> OnGetExpertProfileAsync(int userId, CancellationToken ct)
        {
            var result = await expertAppService.GetByUserId(userId, ct);

            if (!result.IsSuccess || result.Data == null)
                return new JsonResult(null);

            return new JsonResult(result.Data);
        }


        public class ChargeBalanceModel
        {
            public int UserId { get; set; }
            public decimal Amount { get; set; }
            public string? Description { get; set; }
        }
    }
}
