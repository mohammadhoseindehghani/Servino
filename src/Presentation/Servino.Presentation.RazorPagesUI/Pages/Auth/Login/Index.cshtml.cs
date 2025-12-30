using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Dtos.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Servino.Presentation.RazorPagesUI.Pages.Auth.Login
{
    public class IndexModel(IUserAppService userAppService) : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "ایمیل یا شماره موبایل الزامی است")]
            public string UserName { get; set; }

            [DataType(DataType.Password)]
            public string? Password { get; set; }

            public bool RememberMe { get; set; }
        }

        public async Task OnGet(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
                ModelState.AddModelError(string.Empty, ErrorMessage);

            ReturnUrl = returnUrl ?? "/";
            await HttpContext.SignOutAsync();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= "/";

            if (!ModelState.IsValid) return Page();

            var input = Input.UserName.Trim();

            bool isMobile = Regex.IsMatch(input, @"^09\d{9}$");

            if (isMobile)
            {
                var otpResult = await userAppService.SendOtpAsync(new SendOtpDto { MobileNumber = input }, CancellationToken.None);

                if (otpResult.IsSuccess)
                {
                    return RedirectToPage("VerifyOtp", new { mobile = input, returnUrl });
                }

                ModelState.AddModelError("", otpResult.Message ?? "خطا در ارسال پیامک");
                return Page();
            }
            else
            {
                if (string.IsNullOrEmpty(Input.Password))
                {
                    ModelState.AddModelError("Input.Password", "برای ورود با ایمیل، رمز عبور الزامی است.");
                    return Page();
                }

                var loginResult = await userAppService.LoginWithPasswordAsync(new LoginWithPassDto
                {
                    UserName = input,
                    Password = Input.Password
                }, CancellationToken.None);

                if (loginResult.IsSuccess)
                {
                    return LocalRedirect(returnUrl);
                }

                ModelState.AddModelError("", loginResult.Message);
                return Page();
            }
        }
    }
}
