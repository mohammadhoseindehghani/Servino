using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Servino.Domain.Core.LocationAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Dtos;
using Servino.Presentation.RazorPagesUI.Services.File;

namespace Servino.Presentation.RazorPagesUI.Areas.Customer.Pages
{
    [Authorize(Roles = "Customer")]
    public class ProfileModel(
        ICustomerAppService customerAppService,
        IFileService fileService,
        IProvinceAppService provinceAppService,
        ICityAppService cityAppService
        ) : PageModel
    {
        [BindProperty]
        public UpdateCustomerProfileDto Input { get; set; } = new();

        public CustomerProfileDto DisplayData { get; set; } = new();

        public string CityName { get; set; } = "تعیین نشده";

        public SelectList Provinces { get; set; } 

        public int? CurrentProvinceId { get; set; }

        public string? MessageText { get; private set; }
        public string? MessageType { get; private set; } 

        private int GetCurrentUserId()
        {
            var userIdStr = User.FindFirst("UserId")?.Value;
            return int.TryParse(userIdStr, out int userId) ? userId : 0;
        }

        public async Task<IActionResult> OnGetAsync(string? msg, string? text, CancellationToken ct)
        {
            if (!string.IsNullOrEmpty(msg) && !string.IsNullOrEmpty(text))
            {
                MessageType = msg;
                MessageText = text;
            }

            var userId = GetCurrentUserId();
            if (userId == 0) return RedirectToPage("/Auth/Login/Index");

            await LoadData(userId, ct);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? upload, CancellationToken ct)
        {
            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                var errorText = string.Join(" ", errors);

                await LoadData(userId, ct);
                return RedirectToPage(new
                {
                    msg = "danger",
                    text = string.IsNullOrEmpty(errorText) ? "اطلاعات وارد شده معتبر نیست." : errorText
                });
            }

            if (upload != null && upload.Length > 0)
            {
                var newPath = await fileService.Upload(upload, "profiles", ct);
                if (newPath != null)
                {
                    Input.ProfileImagePath = newPath;
                }
            }
            else
            {
                var currentProfile = await customerAppService.GetByUserIdAsync(userId, ct);
                if (currentProfile.IsSuccess)
                {
                    Input.ProfileImagePath = currentProfile.Data.ProfileImagePath;
                }
            }

            Input.UserId = userId;
            var result = await customerAppService.UpdateProfile(Input, ct);

            if (result.IsSuccess)
            {
                return RedirectToPage(new
                {
                    msg = "success",
                    text = "پروفایل شما با موفقیت بروزرسانی شد."
                });
            }
            else
            {
                await LoadData(userId, ct);
                return RedirectToPage(new
                {
                    msg = "danger",
                    text = result.Message ?? "خطایی در بروزرسانی پروفایل رخ داد."
                });
            }
        }

        public async Task<JsonResult> OnGetGetCities(int provinceId, CancellationToken ct)
        {
            var cities = await cityAppService.GetCitiesByProvinceIdAsync(provinceId, ct);
            return new JsonResult(cities);
        }

        private async Task LoadData(int userId, CancellationToken ct)
        {
            var result = await customerAppService.GetByUserIdAsync(userId, ct);
            if (result.IsSuccess && result.Data != null)
            {
                DisplayData = result.Data;
                Input = new UpdateCustomerProfileDto
                {
                    UserId = userId,
                    FirstName = result.Data.FirstName,
                    LastName = result.Data.LastName,
                    CityId = result.Data.CityId,
                    ProfileImagePath = result.Data.ProfileImagePath
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
    }
}
