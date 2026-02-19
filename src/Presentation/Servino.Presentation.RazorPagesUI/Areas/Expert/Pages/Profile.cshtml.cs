using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Servino.Domain.Core._common;
using Servino.Domain.Core.LocationAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Dtos;
using System.ComponentModel.DataAnnotations;
using IFileService = Servino.Presentation.RazorPagesUI.Services.File.IFileService;

namespace Servino.Presentation.RazorPagesUI.Areas.Expert.Pages
{
    [Authorize(Roles = "Expert")]
    public class ProfileModel(
            IExpertAppService expertAppService,
            IUserAppService userAppService,
            IFileService fileService,
            IProvinceAppService provinceAppService,
            ICityAppService cityAppService) : PageModel
    {
        public ExpertProfileDto ExpertProfile { get; set; } = new();

        [BindProperty]
        public UpdateExpertProfileInput Input { get; set; } = new();

        public SelectList Provinces { get; set; } 

        public int? CurrentProvinceId { get; set; }

        public string CityName { get; set; } = "تعیین نشده";

        public string? MessageText { get; private set; }
        public string? MessageType { get; private set; }

        public async Task<IActionResult> OnGet(string? msg, string? text, CancellationToken ct)
        {
            if (!string.IsNullOrEmpty(msg) && !string.IsNullOrEmpty(text))
            {
                MessageType = msg;
                MessageText = text;
            }

            var userId = await GetCurrentUserIdAsync(ct);
            if (userId == 0) return RedirectToPage("/Auth/Login/Index");

            await LoadProfile(userId, ct);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken ct)
        {
            var userId = await GetCurrentUserIdAsync(ct);
            if (userId == 0) return RedirectToPage("/Auth/Login/Index");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                var errorText = string.Join(" ", errors);

                await LoadProfile(userId, ct);
                return RedirectToPage(new
                {
                    msg = "danger",
                    text = string.IsNullOrEmpty(errorText) ? "اطلاعات وارد شده معتبر نیست. لطفاً ورودی‌ها را بررسی کنید." : errorText
                });
            }

            string? imagePath = null;
            var currentProfileResult = await expertAppService.GetByUserId(userId, ct);
            if (currentProfileResult.IsSuccess) imagePath = currentProfileResult.Data.ProfileImagePath;

            if (Input.NewImageFile != null)
            {
                try
                {
                    var newPath = await fileService.Upload(Input.NewImageFile, "avatars", ct);
                    if (!string.IsNullOrEmpty(imagePath)) await fileService.DeleteFile(imagePath, ct);
                    imagePath = newPath;
                }
                catch (Exception ex)
                {
                    await LoadProfile(userId, ct);
                    return RedirectToPage(new
                    {
                        msg = "danger",
                        text = "خطا در آپلود عکس: " + ex.Message
                    });
                }
            }

            var command = new UpdateExpertProfileDto
            {
                UserId = userId,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                CityId = Input.CityId,
                Bio = Input.Bio,
                Address = Input.Address,
                BankCardNumber = Input.BankCardNumber,
                ShebaNumber = Input.ShebaNumber,
                ProfileImagePath = imagePath
            };

            var result = await expertAppService.UpdateProfile(command, ct);

            if (result.IsSuccess)
            {
                return RedirectToPage(new
                {
                    msg = "success",
                    text = "پروفایل با موفقیت بروزرسانی شد."
                });
            }

            await LoadProfile(userId, ct);
            return RedirectToPage(new
            {
                msg = "danger",
                text = result.Message ?? "خطایی در بروزرسانی پروفایل رخ داد."
            });
        }

        public async Task<JsonResult> OnGetGetCities(int provinceId, CancellationToken ct)
        {
            var cities = await cityAppService.GetCitiesByProvinceIdAsync(provinceId, ct);
            return new JsonResult(cities);
        }

        private async Task LoadProfile(int userId, CancellationToken ct)
        {
            var profileResult = await expertAppService.GetByUserId(userId, ct);
            if (profileResult.IsSuccess && profileResult.Data != null)
            {
                ExpertProfile = profileResult.Data;

                Input = new UpdateExpertProfileInput
                {
                    FirstName = ExpertProfile.FirstName,
                    LastName = ExpertProfile.LastName,
                    CityId = ExpertProfile.CityId,
                    Bio = ExpertProfile.Bio,
                    Address = ExpertProfile.Address,
                    BankCardNumber = ExpertProfile.BankCardNumber,
                    ShebaNumber = ExpertProfile.ShebaNumber
                };

                if (Input.CityId.HasValue && Input.CityId > 0)
                {
                    var cityRes = await cityAppService.GetByIdAsync(Input.CityId.Value, ct);
                    if (cityRes.IsSuccess)
                    {
                        CityName = cityRes.Data.Title;
                        CurrentProvinceId = cityRes.Data.ProvinceId;
                    }
                }
            }

            var provincesList = await provinceAppService.GetAllForDropdownAsync(ct);
            Provinces = new SelectList(provincesList, "Id", "Title", CurrentProvinceId);
        }

        private async Task<int> GetCurrentUserIdAsync(CancellationToken ct)
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName)) return 0;

            var search = new PaginationRequestDto { SearchKey = userName };
            var listResult = await userAppService.GetUsersListAsync(search, ct);
            return listResult.IsSuccess && listResult.Data?.Any() == true ? listResult.Data.First().Id : 0;
        }

        public class UpdateExpertProfileInput
        {
            [Required(ErrorMessage = "نام الزامی است")]
            public string? FirstName { get; set; }

            [Required(ErrorMessage = "نام خانوادگی الزامی است")]
            public string? LastName { get; set; }

            public int? CityId { get; set; }

            public string? Bio { get; set; }

            public string? Address { get; set; }

            [MaxLength(16, ErrorMessage = "شماره کارت باید ۱۶ رقم باشد")]
            [RegularExpression(@"^\d{16}$", ErrorMessage = "شماره کارت باید ۱۶ رقم باشد")]
            public string? BankCardNumber { get; set; }

            [MaxLength(26, ErrorMessage = "شماره شبا معتبر نیست")]
            [RegularExpression(@"^\d{24}$", ErrorMessage = "شماره شبا باید ۲۴ رقم (بدون IR) باشد")]
            public string? ShebaNumber { get; set; }

            public IFormFile? NewImageFile { get; set; }
        }
    }
}