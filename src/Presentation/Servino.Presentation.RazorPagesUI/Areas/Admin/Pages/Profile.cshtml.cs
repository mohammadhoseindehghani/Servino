using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Dtos;
using Servino.Domain.Core.UserAgg.Dtos.Identity;
using Servino.Presentation.RazorPagesUI.Services.File;

namespace Servino.Presentation.RazorPagesUI.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class ProfileModel(IUserAppService userAppService, IFileService fileService) : PageModel
    {
        [BindProperty]
        public UserProfileDto Profile { get; set; } = new();

        [BindProperty]
        public UpdateUserDto UpdateCommand { get; set; } = new();

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        private int GetCurrentUserId()
        {
            var userIdStr = User.FindFirst("userId")?.Value;
            if (int.TryParse(userIdStr, out int userId)) return userId;
            return 0;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = GetCurrentUserId();
            if (userId == 0) return RedirectToPage("/Auth/Login/Index");

            await LoadProfile(userId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();

            UpdateCommand.Id = userId;

            if (Upload != null && Upload.Length > 0)
            {
                if (Upload.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("Upload", "حجم فایل بیشتر از ۵ مگابایت است.");
                    await LoadProfile(userId);
                    return Page();
                }

                var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
                var extension = Path.GetExtension(Upload.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Upload", "فرمت فایل مجاز نیست.");
                    await LoadProfile(userId);
                    return Page();
                }

                var currentProfile = await userAppService.GetUserProfileAsync(userId, "Admin", CancellationToken.None);

                if (currentProfile.IsSuccess
                    && !string.IsNullOrEmpty(currentProfile.Data.ProfileImagePath)
                    && currentProfile.Data.ProfileImagePath.StartsWith("/Files/"))
                {
                    await fileService.DeleteFile(currentProfile.Data.ProfileImagePath, CancellationToken.None);
                }

                var newPath = await fileService.Upload(Upload, "profiles", CancellationToken.None);
                if (newPath == null)
                {
                    ErrorMessage = "خطا در آپلود فایل.";
                    await LoadProfile(userId);
                    return Page();
                }
                UpdateCommand.ProfileImagePath = newPath;
            }

            var updateResult = await userAppService.UpdateUserProfileAsync(UpdateCommand, CancellationToken.None);

            if (!updateResult.IsSuccess)
            {
                ErrorMessage = updateResult.Message ?? "خطا در ذخیره تغییرات.";
                await LoadProfile(userId);
                return Page();
            }

            SuccessMessage = "پروفایل با موفقیت بروزرسانی شد.";
            await LoadProfile(userId);
            return Page();
        }

        private async Task LoadProfile(int userId)
        {
            var result = await userAppService.GetUserProfileAsync(userId, "Admin", CancellationToken.None);

            if (result.IsSuccess && result.Data != null)
            {
                Profile = result.Data;

                UpdateCommand = new UpdateUserDto()
                {
                    Id = Profile.Id,
                    FirstName = Profile.FirstName,
                    LastName = Profile.LastName,
                    CityId = Profile.CityId,
                    ProfileImagePath = Profile.ProfileImagePath
                };
            }
            else
            {
                ErrorMessage = "اطلاعات پروفایل یافت نشد.";
                Profile = new UserProfileDto();
                UpdateCommand = new UpdateUserDto() { Id = userId };
            }
        }
    }
}
