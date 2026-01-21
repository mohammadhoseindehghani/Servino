using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Presentation.RazorPagesUI.Areas.Customer.Pages
{
    [Authorize(Roles = "Customer")]
    public class SecurityModel(IUserAppService userAppService) : PageModel
    {
        [BindProperty]
        public string CurrentPassword { get; set; } = default!;

        [BindProperty]
        public string NewPassword { get; set; } = default!;

        [BindProperty]
        public string ConfirmPassword { get; set; } = default!;

        public async Task<IActionResult> OnPostChangePasswordAsync(CancellationToken ct)
        {
            if (NewPassword != ConfirmPassword)
            {
                ModelState.AddModelError("", "رمز جدید و تکرار آن یکسان نیست.");
                return Page();
            }

            var userId = int.Parse(User.FindFirst("UserId")!.Value);

            var result = await userAppService.ChangePasswordAsync(
                new ChangePasswordDto
                {
                    UserId = userId,
                    CurrentPassword = CurrentPassword,
                    NewPassword = NewPassword
                },
                ct
            );

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Message!);
                return Page();
            }

            TempData["Success"] = result.Message;
            return RedirectToPage();
        }
    }
}
