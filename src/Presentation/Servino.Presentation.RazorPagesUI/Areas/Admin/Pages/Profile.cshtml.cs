using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Presentation.RazorPagesUI.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class ProfileModel(IAdminAppService adminAppService) : PageModel
    {
        [BindProperty]
        public AdminProfileDto Profile { get; set; } = new();

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


        private async Task LoadProfile(int userId)
        {
            var result = await adminAppService.GetByUserIdAsync(userId, CancellationToken.None);

            if (result.IsSuccess && result.Data != null)
            {
                Profile = result.Data;
            }
            else
            {
                ErrorMessage = "اطلاعات پروفایل یافت نشد.";
                Profile = new AdminProfileDto();
                UpdateCommand = new UpdateUserDto() { Id = userId };
            }
        }
    }
}
