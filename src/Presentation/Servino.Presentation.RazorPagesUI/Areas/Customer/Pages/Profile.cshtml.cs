using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Dtos;
using Servino.Domain.Core.UserAgg.Dtos.Identity;
using Servino.Presentation.RazorPagesUI.Services.File;

namespace Servino.Presentation.RazorPagesUI.Areas.Customer.Pages
{
    [Authorize(Roles = "Customer")]
    public class ProfileModel(IUserAppService userAppService, IFileService fileService) : PageModel
    {
        public UserProfileDto Profile { get; set; } = new();

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        private int GetCurrentUserId()
        {
            var userIdStr = User.FindFirst("userId")?.Value;
            if (int.TryParse(userIdStr, out int userId))
                return userId;
            return 0;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = GetCurrentUserId();
            if (userId == 0) return RedirectToPage("/Auth/Login/Index");

            await LoadProfile(userId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string firstName, string lastName, int cityId, IFormFile? upload)
        {
            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();

            if (string.IsNullOrWhiteSpace(firstName))
            {
                ErrorMessage = "نام الزامی است.";
                await LoadProfile(userId);
                return Page();
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                ErrorMessage = "نام خانوادگی الزامی است.";
                await LoadProfile(userId);
                return Page();
            }

            if (cityId == 0)
            {
                ErrorMessage = "انتخاب شهر الزامی است.";
                await LoadProfile(userId);
                return Page();
            }

            var currentProfileResult = await userAppService.GetUserProfileAsync(userId, "Customer", CancellationToken.None);
            string? profileImagePath = currentProfileResult.IsSuccess ? currentProfileResult.Data?.ProfileImagePath : null;

            if (upload != null && upload.Length > 0)
            {
                if (upload.Length > 5 * 1024 * 1024)
                {
                    ErrorMessage = "حجم فایل بیشتر از ۵ مگابایت است.";
                    await LoadProfile(userId);
                    return Page();
                }

                var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
                var extension = Path.GetExtension(upload.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ErrorMessage = "فرمت فایل مجاز نیست (فقط JPG, JPEG, PNG).";
                    await LoadProfile(userId);
                    return Page();
                }

                if (!string.IsNullOrEmpty(profileImagePath) && profileImagePath.StartsWith("/Files/"))
                {
                    await fileService.DeleteFile(profileImagePath, CancellationToken.None);
                }

                var newPath = await fileService.Upload(upload, "profiles", CancellationToken.None);
                if (newPath == null)
                {
                    ErrorMessage = "خطا در آپلود فایل.";
                    await LoadProfile(userId);
                    return Page();
                }

                profileImagePath = newPath;
            }

            var updateCommand = new UpdateProfileDto
            {
                Id = userId,
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                CityId = cityId,
                ProfileImagePath = profileImagePath
            };

            //var updateResult = await userAppService.UpdateUserProfileAsync(updateCommand, "Customer", CancellationToken.None);

            //if (!updateResult.IsSuccess)
            //{
            //    ErrorMessage = updateResult.Message ?? "خطا در ذخیره تغییرات.";
            //    await LoadProfile(userId);
            //    return Page();
            //}

            SuccessMessage = "پروفایل با موفقیت بروزرسانی شد.";
            return RedirectToPage();
        }

        private async Task LoadProfile(int userId)
        {
            var result = await userAppService.GetUserProfileAsync(userId, "Customer", CancellationToken.None);

            if (result.IsSuccess && result.Data != null)
            {
                Profile = result.Data;
            }
            else
            {
                ErrorMessage = "اطلاعات پروفایل یافت نشد.";
                Profile = new UserProfileDto();
            }
        }
    }
}
