using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core._common;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Dtos;
using Servino.Presentation.RazorPagesUI.Services.File;
using System.ComponentModel.DataAnnotations;

namespace Servino.Presentation.RazorPagesUI.Areas.Expert.Pages
{
    [Authorize(Roles = "Expert")]
    public class ProfileModel(
            IExpertAppService expertAppService,
            IUserAppService userAppService,
            IFileService fileService) : PageModel
    {
        public ExpertProfileDto ExpertProfile { get; set; } = new();

        [BindProperty]
        public UpdateExpertProfileInput Input { get; set; } = new();

        [TempData] public string? SuccessMessage { get; set; }
        [TempData] public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGet(CancellationToken ct)
        {
            var userId = await GetCurrentUserIdAsync(ct);
            if (userId == 0) return RedirectToPage("/Auth/Login/Index");

            var profileResult = await expertAppService.GetByUserId(userId, ct);
            if (!profileResult.IsSuccess) return RedirectToPage("/Index");

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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken ct)
        {
            var userId = await GetCurrentUserIdAsync(ct);
            if (userId == 0) return RedirectToPage("/Auth/Login/Index");

            if (!ModelState.IsValid)
            {
                ErrorMessage = "اطلاعات وارد شده معتبر نیست. لطفاً ورودی‌ها را بررسی کنید.";
                var profileResult = await expertAppService.GetByUserId(userId, ct);
                if (profileResult.IsSuccess) ExpertProfile = profileResult.Data;
                return Page();
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
                    ErrorMessage = "خطا در آپلود عکس: " + ex.Message;
                    if (currentProfileResult.IsSuccess) ExpertProfile = currentProfileResult.Data;
                    return Page();
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
                SuccessMessage = "پروفایل با موفقیت بروزرسانی شد.";
                return RedirectToPage();
            }

            ErrorMessage = result.Message;
            if (currentProfileResult.IsSuccess) ExpertProfile = currentProfileResult.Data;
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
            public string? BankCardNumber { get; set; }

            [MaxLength(26, ErrorMessage = "شماره شبا معتبر نیست")]
            public string? ShebaNumber { get; set; }

            public IFormFile? NewImageFile { get; set; }
        }
    }
}
