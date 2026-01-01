using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Dtos.Identity;
using System.ComponentModel.DataAnnotations;

namespace Servino.Presentation.RazorPagesUI.Pages.Auth.Login
{
    public class VerifyOtpModel(IUserAppService userAppService) : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string Mobile { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; } = string.Empty;

        public class InputModel
        {
            [Required(ErrorMessage = "کد تأیید الزامی است")]
            [Display(Name = "کد تأیید")]
            public string Code { get; set; } = string.Empty;
        }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(Mobile))
            {
                return RedirectToPage("/Auth/Login");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await userAppService.VerifyOtpAndLoginAsync(new LoginWithOtpDto
            {
                MobileNumber = Mobile,
                Code = Input.Code
            }, CancellationToken.None);

            if (!result.IsSuccess || result.Data?.Token == null)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "کد تأیید نامعتبر است.");
                return Page();
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(8) 
            };

            Response.Cookies.Append("access_token", result.Data.Token, cookieOptions);

            return LocalRedirect(string.IsNullOrEmpty(ReturnUrl) ? "/" : ReturnUrl);
        }
    }
}
