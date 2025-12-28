using Microsoft.AspNetCore.Identity;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Dtos.Identity;
using System.Security.Claims;

namespace Servino.Infa.Db.SqlServer.EfCore.Identity.Service;

public class IdentityService(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager)
    : IIdentityService
{


    // private readonly IJwtTokenGenerator _jwtTokenGenerator; 

  
    public async Task<LoginResultDto> LoginWithPasswordAsync(LoginWithPassDto loginDto, CancellationToken ct)
    {
        IdentityUser? user;

        if (loginDto.UserName.Contains("@"))
        {
            user = await userManager.FindByEmailAsync(loginDto.UserName);
        }
        else
        {
            user = userManager.Users.FirstOrDefault(u => u.PhoneNumber == loginDto.UserName);
        }

        if (user == null)
            return new LoginResultDto { Succeeded = false, Message = "کاربری با این مشخصات یافت نشد." };

        // if (await _userManager.IsLockedOutAsync(user)) ...

        var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
            return new LoginResultDto { Succeeded = false, Message = "رمز عبور اشتباه است." };

        // var token = _jwtTokenGenerator.GenerateToken(user);
        return new LoginResultDto { Succeeded = true, Token = "JWT_TOKEN_HERE_SAMPLE" };
    }


    public async Task<string> SendOtpAsync(SendOtpDto otpDto, CancellationToken ct)
    {
        var user = userManager.Users.FirstOrDefault(u => u.PhoneNumber == otpDto.MobileNumber);

        var code = new Random().Next(1000, 9999).ToString();

        // *** TODO: ذخیره کد در کش (Redis/Memory) ***
        // await _cache.SetStringAsync($"OTP_{otpDto.MobileNumber}", code, TimeSpan.FromMinutes(2));

        // *** TODO: ارسال پیامک ***
        // await _smsService.SendAsync(otpDto.MobileNumber, code);

        return $"کد تایید برای تست: {code}";
    }


    public async Task<LoginResultDto> VerifyOtpAndLoginAsync(LoginWithOtpDto otpDto, CancellationToken ct)
    {
        // 1. اعتبارسنجی کد (فعلاً فرض می‌کنیم درست است)
        // var cachedCode = await _cache.GetStringAsync($"OTP_{otpDto.MobileNumber}");
        // if (cachedCode != otpDto.Code) return ...

        var user = userManager.Users.FirstOrDefault(u => u.PhoneNumber == otpDto.MobileNumber);

        if (user == null)
        {
            user = new IdentityUser
            {
                UserName = otpDto.MobileNumber,
                PhoneNumber = otpDto.MobileNumber,
                PhoneNumberConfirmed = true,
                EmailConfirmed = true 
            };

            var createResult = await userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                var errorMsg = string.Join(", ", createResult.Errors.Select(e => e.Description));
                return new LoginResultDto { Succeeded = false, Message = $"خطا در ثبت نام: {errorMsg}" };
            }

            await userManager.AddToRoleAsync(user, "Customer");
        }

        return new LoginResultDto { Succeeded = true, Token = "JWT_TOKEN_HERE_SAMPLE" };
    }

    public async Task<LoginResultDto> LoginWithGoogleAsync(LoginWithGoogleDto googleDto, CancellationToken ct)
    {
        return new LoginResultDto { Succeeded = true, Token = "JWT_TOKEN_FROM_GOOGLE_LOGIN" };
    }

    public async Task DeleteUserAsync(string id, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with Id {id} not found.");

        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"خطا در حذف کاربر: {errors}");
        }
    }



    public async Task<IdentityResultDto> RegisterWithEmailAsync(RegisterDto registerDto, CancellationToken ct)
    {
        var existingUser = await userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null)
            return new IdentityResultDto { Succeeded = false, Message = "این ایمیل قبلاً ثبت شده است." };

        var user = new IdentityUser
        {
            UserName = registerDto.Email, 
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber
        };

        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new IdentityResultDto { Succeeded = false, Message = errors };
        }

        if (!string.IsNullOrEmpty(registerDto.Role))
        {
            await userManager.AddToRoleAsync(user, registerDto.Role);
        }

        return new IdentityResultDto { Succeeded = true, Id = user.Id, Message = "ثبت نام با موفقیت انجام شد." };
    }
}