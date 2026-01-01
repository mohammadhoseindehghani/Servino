using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
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
        public InputModel Input { get; set; } = new();

        public string? ReturnUrl { get; set; }
        public string? ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "ایمیل یا شماره موبایل الزامی است")]
            [Display(Name = "ایمیل یا موبایل")]
            public string UserName { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "رمز عبور")]
            public string? Password { get; set; }

            [Display(Name = "مرا به خاطر بسپار")]
            public bool RememberMe { get; set; }
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? "/";
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= "/";

            if (!ModelState.IsValid) return Page();

            var input = Input.UserName.Trim();
            var isMobile = Regex.IsMatch(input, @"^09\d{9}$");

            if (isMobile)
            {
                var otpResult = await userAppService.SendOtpAsync(new SendOtpDto { MobileNumber = input }, CancellationToken.None);
                if (otpResult.IsSuccess)
                    return RedirectToPage("VerifyOtp", new { mobile = input, returnUrl });

                ModelState.AddModelError("", otpResult.Message ?? "خطا");
                return Page();
            }

            if (string.IsNullOrEmpty(Input.Password))
            {
                ModelState.AddModelError("Input.Password", "رمز عبور الزامی است.");
                return Page();
            }

            var result = await userAppService.LoginWithPasswordAsync(new LoginWithPassDto
            {
                UserName = input,
                Password = Input.Password
            }, CancellationToken.None);

            if (result.IsSuccess)
            {
                string redirectUrl = result.Data.Role?.ToLower() switch
                {
                    "admin" => "/admin/profile",
                    "customer" => "/customer/profile",
                    "expert" => "/expert/profile",
                    _ => returnUrl
                };
                return LocalRedirect(redirectUrl);
            }

            ModelState.AddModelError(string.Empty, result.Message ?? "ورود ناموفق بود.");
            return Page();
        }
    }
}
