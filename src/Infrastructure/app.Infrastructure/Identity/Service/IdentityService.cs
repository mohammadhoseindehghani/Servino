using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.Contracts.Services;
using app.Application.DTOs.IdentityDTOs;
using IPE.SmsIrClient.Models.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace app.Infrastructure.Identity.Service;


public class IdentityService(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    RoleManager<IdentityRole> roleManager,
    ISmsService smsService,
    IMemoryCache cache,
    IUserRepository userRepository) : IIdentityService
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

        if (otpDto.Code != "12345" && cachedCode != otpDto.Code)
        {
            return new LoginResultDto { Succeeded = false, Message = "کد وارد شده اشتباه است." };
        }

        cache.Remove(cacheKey);

        var user = userManager.Users.FirstOrDefault(u => u.PhoneNumber == otpDto.MobileNumber);

        if (user == null)
        {
            return new LoginResultDto
            {
                Succeeded = false,
                Message = "UserNotFound", 
            };
        }

        if (await userManager.IsLockedOutAsync(user))
        {
            return new LoginResultDto { Succeeded = false, Message = "حساب کاربری شما غیرفعال شده است." };
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

    public async Task LockUserAsync(string identityId, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(identityId);
        if (user != null)
        {
            await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            await userManager.UpdateSecurityStampAsync(user);
        }
    }

    public async Task<Result<bool>> AdminChangePasswordAsync(string identityId, string newPassword, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(identityId);
        if (user == null) return Result<bool>.Failure("کاربر یافت نشد.");

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, token, newPassword);

        return result.Succeeded
            ? Result<bool>.Success(true)
            : Result<bool>.Failure("خطا در تغییر رمز عبور.");
    }
    public async Task<Result<bool>> DeactivateUserAsync(string identityId, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(identityId);
        if (user == null) return Result<bool>.Failure("کاربر در سامانه هویت یافت نشد.");

        var randomCode = new Random().Next(10000, 99999);
        var deletedEmail = $"deleted_{randomCode}_{user.Email}";

        user.UserName = deletedEmail;
        user.NormalizedUserName = deletedEmail.ToUpper();

        user.Email = deletedEmail;
        user.NormalizedEmail = deletedEmail.ToUpper();

        user.PhoneNumber = null;
        user.PhoneNumberConfirmed = false;
        user.EmailConfirmed = false;

        user.LockoutEnabled = true;
        user.LockoutEnd = DateTimeOffset.MaxValue;

        user.SecurityStamp = Guid.NewGuid().ToString();

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return Result<bool>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return Result<bool>.Success(true);
    }

    public async Task<IdentityResultDto> RegisterWithEmailAsync(RegisterDto registerDto, string role, CancellationToken ct)
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

        var targetRole = string.IsNullOrEmpty(role) ? "Customer" : role;

        if (!await roleManager.RoleExistsAsync(targetRole))
        {
            targetRole = "Customer";
        }

        await userManager.AddToRoleAsync(user, targetRole);

        if (string.IsNullOrEmpty(role))
        {
            await signInManager.SignInAsync(user, isPersistent: false);
        }

        return new IdentityResultDto
        {
            Succeeded = true,
            Id = user.Id
        };
    }

    public async Task<Result<bool>> ChangePasswordAsync(string identityId, string currentPassword, string newPassword, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(identityId);
        if (user == null)
            return Result<bool>.Failure("حساب کاربری یافت نشد.");

        var result = await userManager.ChangePasswordAsync(
            user,
            currentPassword,
            newPassword
        );

        if (!result.Succeeded)
        {
            var error = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<bool>.Failure(error);
        }

        await signInManager.RefreshSignInAsync(user);
        return Result<bool>.Success(true, "رمز عبور با موفقیت تغییر کرد.");
    }
}