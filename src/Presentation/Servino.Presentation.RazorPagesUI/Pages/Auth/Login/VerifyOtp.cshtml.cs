using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Dtos.Identity;
using System.ComponentModel.DataAnnotations;

namespace Servino.Presentation.RazorPagesUI.Pages.Auth.Login
{
    public class VerifyOtpModel(IUserAppService userAppService) : PageModel
    {
        [BindProperty] public InputModel Input { get; set; }

        [BindProperty(SupportsGet = true)] public string Mobile { get; set; }
        [BindProperty(SupportsGet = true)] public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            public string Code { get; set; }
        }

        public void OnGet()
        {
            if (string.IsNullOrEmpty(Mobile)) Response.Redirect("/Auth/Login");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var result = await userAppService.VerifyOtpAndLoginAsync(new LoginWithOtpDto
            {
                MobileNumber = Mobile,
                Code = Input.Code
            }, CancellationToken.None);

            if (result.IsSuccess)
            {
                return LocalRedirect(string.IsNullOrEmpty(ReturnUrl) ? "/" : ReturnUrl);
            }

            ModelState.AddModelError("", result.Message ?? "کد نامعتبر است");
            return Page();
        }
    }
}
