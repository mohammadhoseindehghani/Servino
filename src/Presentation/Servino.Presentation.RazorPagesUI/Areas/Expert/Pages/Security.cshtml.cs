using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Presentation.RazorPagesUI.Areas.Expert.Pages
{
    [Authorize(Roles = "Expert")]
    public class SecurityModel(IUserAppService userAppService) : PageModel
    {
        [BindProperty]
        public string CurrentPassword { get; set; } 

        [BindProperty]
        public string NewPassword { get; set; } 

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public string? MessageText { get; private set; }
        public string? MessageType { get; private set; } 

        public async Task<IActionResult> OnGet(string? msg, string? text, CancellationToken ct)
        {
            if (!string.IsNullOrEmpty(msg) && !string.IsNullOrEmpty(text))
            {
                MessageType = msg;
                MessageText = text;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostChangePasswordAsync(CancellationToken ct)
        {
            if (NewPassword != ConfirmPassword)
            {
                return RedirectToPage(new
                {
                    msg = "danger",
                    text = "رمز جدید و تکرار آن یکسان نیست."
                });
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
                return RedirectToPage(new
                {
                    msg = "danger",
                    text = result.Message ?? "خطایی در تغییر رمز عبور رخ داد."
                });
            }

            return RedirectToPage(new
            {
                msg = "success",
                text = result.Message ?? "رمز عبور با موفقیت بروزرسانی شد."
            });
        }
    }
}
