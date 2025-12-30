using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Dtos.Identity;
using IPE.SmsIrClient.Models.Requests;
using Servino.Infra.Providers.SmsProvider.SmsIrService;

namespace Servino.Infa.Db.SqlServer.EfCore.Identity.Service;

public class IdentityService(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    ISmsService smsService,
    IMemoryCache cache) : IIdentityService
{
    public async Task<string> SendOtpAsync(SendOtpDto otpDto, CancellationToken ct)
    {
        var code = new Random().Next(10000, 99999).ToString();

        var cacheKey = $"OTP_{otpDto.MobileNumber}";
        cache.Set(cacheKey, code, TimeSpan.FromMinutes(2));

        var smsParams = new List<VerifySendParameter>
        {
            new VerifySendParameter("Code", code) 
        };

        await smsService.SendOtpAsync(otpDto.MobileNumber, 753409, smsParams);

        return "کد ارسال شد";
    }

    public async Task<LoginResultDto> VerifyOtpAndLoginAsync(LoginWithOtpDto otpDto, CancellationToken ct)
    {
        var cacheKey = $"OTP_{otpDto.MobileNumber}";

        if (!cache.TryGetValue(cacheKey, out string? cachedCode))
        {
            return new LoginResultDto { Succeeded = false, Message = "کد منقضی شده است." };
        }

        if (cachedCode != otpDto.Code)
        {
            return new LoginResultDto { Succeeded = false, Message = "کد اشتباه است." };
        }

        cache.Remove(cacheKey);

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

            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
                return new LoginResultDto { Succeeded = false, Message = "خطا در ثبت کاربر جدید." };

            await userManager.AddToRoleAsync(user, "Customer");
        }

        await signInManager.SignInAsync(user, isPersistent: true);

        return new LoginResultDto { Succeeded = true, Token = "JWT_TOKEN" };
    }

    public async Task<LoginResultDto> LoginWithPasswordAsync(LoginWithPassDto loginDto, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(loginDto.UserName);
        if (user == null)
            return new LoginResultDto { Succeeded = false, Message = "کاربری با این ایمیل یافت نشد." };

        var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
            return new LoginResultDto { Succeeded = false, Message = "رمز عبور اشتباه است." };

        return new LoginResultDto { Succeeded = true, Token = "JWT_TOKEN" };
    }

    public async Task<LoginResultDto> LoginWithGoogleAsync(LoginWithGoogleDto googleDto, CancellationToken ct)
    {
        return new LoginResultDto { Succeeded = true, Token = "GOOGLE_TOKEN" };
    }

    public async Task DeleteUserAsync(string id, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null) throw new KeyNotFoundException($"User {id} not found.");
        await userManager.DeleteAsync(user);
    }

    public async Task<IdentityResultDto> RegisterWithEmailAsync(RegisterDto registerDto, CancellationToken ct)
    {
        return new IdentityResultDto { Succeeded = true };
    }
}