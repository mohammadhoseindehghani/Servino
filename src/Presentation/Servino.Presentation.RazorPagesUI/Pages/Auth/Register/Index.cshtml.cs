using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Dtos.Identity;
using System.ComponentModel.DataAnnotations;

namespace Servino.Presentation.RazorPagesUI.Pages.Auth.Register
{
    public class IndexModel(IUserAppService userAppService) : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "لطفا نام کاربری را وارد کنید")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "لطفا ایمیل را وارد کنید")]
            [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نیست")]
            public string Email { get; set; }

            [Required(ErrorMessage = "لطفا شماره موبایل را وارد کنید")]
            [RegularExpression(@"^09\d{9}$", ErrorMessage = "شماره موبایل نامعتبر است (مثلا 09120000000)")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "لطفا رمز عبور را وارد کنید")]
            [StringLength(100, ErrorMessage = "{0} باید حداقل {2} کاراکتر باشد.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required(ErrorMessage = "لطفا نقش خود را انتخاب کنید")]
            public string Role { get; set; } 

            [Range(typeof(bool), "true", "true", ErrorMessage = "پذیرش قوانین الزامی است")]
            public bool TermsAccepted { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var registerDto = new RegisterDto
            {
                UserName = Input.UserName,
                Email = Input.Email,
                PhoneNumber = Input.PhoneNumber,
                Password = Input.Password,
                Role = Input.Role
            };

            var result = await userAppService.RegisterUserAsync(registerDto, CancellationToken.None);

            if (result.IsSuccess)
            {
   
                return RedirectToPage("/Auth/Login/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "خطا در ثبت نام.");
                return Page();
            }
        }
    }
}
