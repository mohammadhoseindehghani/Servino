
using IPE.SmsIrClient.Models.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

using Servino.Domain.Core._common;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Dtos.Identity;
using Servino.Infra.Providers.SmsProvider.SmsIrService;

namespace Servino.Infa.Db.SqlServer.EfCore.Identity.Service;


public class IdentityService(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager, 
    ISmsService smsService,
    IMemoryCache cache,
    IUserService userService) : IIdentityService
{

    public async Task<LoginResultDto> LoginWithPasswordAsync(LoginWithPassDto loginDto, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(loginDto.UserName);
        if (user == null)
            return new LoginResultDto { Succeeded = false, Message = "کاربری با این ایمیل یافت نشد." };

        var result = await signInManager.PasswordSignInAsync(user, loginDto.Password, isPersistent: true, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
                return new LoginResultDto { Succeeded = false, Message = "حساب کاربری قفل شده است." };

            return new LoginResultDto { Succeeded = false, Message = "رمز عبور اشتباه است." };
        }

        var roles = await userManager.GetRolesAsync(user);
        string role = roles.FirstOrDefault() ?? "Customer";

        return new LoginResultDto
        {
            Succeeded = true,
            Role = role
        };
    }


    public async Task<string> SendOtpAsync(SendOtpDto otpDto, CancellationToken ct)
    {
        var code = new Random().Next(10000, 99999).ToString();
        var cacheKey = $"OTP_{otpDto.MobileNumber}";

        cache.Set(cacheKey, code, TimeSpan.FromMinutes(2));

        Console.WriteLine($" >>> OTP CODE FOR {otpDto.MobileNumber} : {code} <<< ");

         var smsParams = new List<VerifySendParameter> { new VerifySendParameter("Code", code) };
         await smsService.SendOtpAsync(otpDto.MobileNumber, 753409, smsParams);

        return "کد تایید ارسال شد";
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
            return new LoginResultDto { Succeeded = false, Message = "کد وارد شده اشتباه است." };
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

            var createResult = await userManager.CreateAsync(user);
            if (!createResult.Succeeded)
                return new LoginResultDto { Succeeded = false, Message = "خطا در ثبت کاربر جدید." };

            await userManager.AddToRoleAsync(user, "Customer");

        }

        await signInManager.SignInAsync(user, isPersistent: true);

        var roles = await userManager.GetRolesAsync(user);
        string role = roles.FirstOrDefault() ?? "Customer";

        return new LoginResultDto
        {
            Succeeded = true,
            Role = role
        };
    }


    public async Task<IdentityResultDto> RegisterWithEmailAsync(RegisterDto registerDto, CancellationToken ct)
    {
        var user = new IdentityUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return new IdentityResultDto
            {
                Succeeded = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }

        await userManager.AddToRoleAsync(user, "Customer");

        await signInManager.SignInAsync(user, isPersistent: false);

        return new IdentityResultDto
        {
            Succeeded = true,
            Id = user.Id
        };
    }

    public async Task LogoutAsync()
    {
        await signInManager.SignOutAsync();
    }

    public async Task DeleteUserAsync(string id, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User {id} not found.");

        await userManager.DeleteAsync(user);
    }

    public async Task<Result<bool>> ChangeEmailAsync(string identityId, string newEmail, CancellationToken ct)
    {
        var identityUser = await userManager.FindByIdAsync(identityId);
        if (identityUser == null)
            return Result<bool>.Failure("حساب کاربری یافت نشد.");

        var existingUser = await userManager.FindByEmailAsync(newEmail);
        if (existingUser != null && existingUser.Id != identityId)
            return Result<bool>.Failure("این ایمیل قبلاً استفاده شده است.");

        identityUser.Email = newEmail;
        identityUser.UserName = newEmail;
        identityUser.EmailConfirmed = false;

        var result = await userManager.UpdateAsync(identityUser);
        if (!result.Succeeded)
            return Result<bool>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));

        await signInManager.RefreshSignInAsync(identityUser);

        return Result<bool>.Success(true, "ایمیل با موفقیت تغییر کرد.");
    }

    public async Task<string?> GetEmailByIdentityIdAsync(string identityId, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(identityId);
        return user?.Email;
    }

    public async Task<LoginResultDto> LoginWithGoogleAsync(LoginWithGoogleDto googleDto, CancellationToken ct)
    {
        return new LoginResultDto
        {
            Succeeded = false,
            Message = "سرویس گوگل هنوز فعال نشده است."
        };
    }
}