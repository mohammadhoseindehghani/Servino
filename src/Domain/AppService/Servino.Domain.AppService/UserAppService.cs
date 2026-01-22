using Servino.Domain.Core._common;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Dtos;
using Servino.Domain.Core.UserAgg.Dtos.Identity;
using Microsoft.Extensions.Logging;


namespace Servino.Domain.AppService;

public class UserAppService(
    IIdentityService identityService,
    IUserService userService,
    IExpertService expertService,
    ICustomerService customerService,
    ILogger<UserAppService> logger) : IUserAppService
{

    public async Task<Result<bool>> CreateUserByAdminAsync(CreateUserByAdminDto command, CancellationToken ct)
    {
        if (await userService.IsEmailExistAsync(command.Email, ct))
            return Result<bool>.Failure("این ایمیل قبلاً ثبت شده است.");

        var registerDto = new RegisterDto
        {
            Email = command.Email,
            Password = command.Password,
            PhoneNumber = command.Mobile
        };

        var identityResult = await identityService.RegisterWithEmailAsync(registerDto, command.Role, ct);

        if (!identityResult.Succeeded)
            return Result<bool>.Failure(identityResult.Message ?? "خطا در سیستم هویت‌سنجی");

        try
        {
            var createUserDto = new CreateUserDto
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                Mobile = command.Mobile,
                IdentityId = identityResult.Id!,
                CityId = 1 
            };

            var userCreated = await userService.CreateAsync(createUserDto, ct);
            if (!userCreated) throw new Exception("خطا در ذخیره کاربر");

            var userId = await userService.GetIdByIdentityIdAsync(identityResult.Id!, ct);

            switch (command.Role)
            {
                case "Expert":
                    await expertService.CreateAsync(userId, ct);
                    break;
                case "Customer":
                    await customerService.CreateAsync(userId, ct);
                    break;
                case "Admin":

                    break;
            }

            return Result<bool>.Success(true, "کاربر با موفقیت ایجاد شد.");
        }
        catch (Exception ex)
        {
            await identityService.DeleteUserAsync(identityResult.Id!, ct);
            return Result<bool>.Failure($"خطای سیستمی: {ex.Message}");
        }
    }

    public async Task<Result<bool>> RegisterUserAsync(RegisterDto command, CancellationToken ct)
    {
        if (await userService.IsEmailExistAsync(command.Email, ct))
        {
            return Result<bool>.Failure("این ایمیل قبلاً در سیستم ثبت شده است.");
        }

        var identityResult = await identityService.RegisterWithEmailAsync(command, ct);

        if (!identityResult.Succeeded)
        {
            return Result<bool>.Failure(identityResult.Message ?? "خطا در ثبت نام امنیتی");
        }

        var createUserDto = new CreateUserDto
        {
            FirstName = "کاربر", 
            LastName = "جدید",
            Email = command.Email,
            Mobile = command.PhoneNumber,
            IdentityId = identityResult.Id!, 
        };

        try
        {
            var dbResult = await userService.CreateAsync(createUserDto, ct);

            if (!dbResult)
            {
                await identityService.DeleteUserAsync(identityResult.Id, ct);
                return Result<bool>.Failure("خطا در ذخیره اطلاعات کاربری.");
            }

            return Result<bool>.Success(true, "ثبت نام با موفقیت انجام شد.");
        }
        catch (Exception ex)
        {
            await identityService.DeleteUserAsync(identityResult.Id, ct);
            return Result<bool>.Failure($"خطای سیستمی: {ex.Message}");
        }
    }

    public async Task<Result<LoginResultDto>> LoginWithPasswordAsync(LoginWithPassDto command, CancellationToken ct)
    {
        var result = await identityService.LoginWithPasswordAsync(command, ct);

        if (!result.Succeeded)
            return Result<LoginResultDto>.Failure(result.Message ?? "نام کاربری یا رمز عبور اشتباه است.");

        return Result<LoginResultDto>.Success(result);
    }


    public async Task<Result<string>> SendOtpAsync(SendOtpDto command, CancellationToken ct)
    {
        var result = await identityService.SendOtpAsync(command, ct);
        return Result<string>.Success(result, "کد تایید ارسال شد.");
    }

    public async Task<Result<LoginResultDto>> VerifyOtpAndLoginAsync(LoginWithOtpDto command, CancellationToken ct)
    {
        var result = await identityService.VerifyOtpAndLoginAsync(command, ct);

        if (!result.Succeeded)
            return Result<LoginResultDto>.Failure(result.Message ?? "کد وارد شده نامعتبر است.");

        return Result<LoginResultDto>.Success(result);
    }

    public async Task<Result<bool>> UpdateUserProfileAsync(UpdateUserDto command, CancellationToken ct)
    {
        var success = await userService.UpdateProfileAsync(command, ct);
        if (!success)
            return Result<bool>.Failure("خطا در بروزرسانی اطلاعات پروفایل.");

        return Result<bool>.Success(true, "پروفایل با موفقیت بروزرسانی شد.");
    }

    public async Task<Result<bool>> AdminUpdateUserAsync(AdminUpdateUserDto command, CancellationToken ct)
    {
        try
        {
            var existingUser = await userService.GetByIdAsync(command.Id, ct);
            var userUpdateDto = new UpdateUserDto
            {
                Id = command.Id,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Mobile = command.Mobile,
                CityId = (command.CityId > 0) ? command.CityId : existingUser.CityId
                // ProfileImagePath 
            };

            var basicUpdateResult = await userService.UpdateAsync(userUpdateDto, ct);
            if (!basicUpdateResult)
            {
                return Result<bool>.Failure("کاربر یافت نشد یا ویرایش اطلاعات پایه انجام نشد.");
            }

            if (command.Role == "Expert" && command.ExpertInfo != null)
            {
                var expertDto = new UpdateExpertProfileDto
                {
                    UserId = command.Id,
                    Bio = command.ExpertInfo.Bio,
                    Address = command.ExpertInfo.Address,
                    BankCardNumber = command.ExpertInfo.BankCardNumber,
                    ShebaNumber = command.ExpertInfo.ShebaNumber,

                    FirstName = null,
                    LastName = null,
                    CityId = null
                };

                await expertService.UpdateProfile(expertDto, ct);
            }

            if (!string.IsNullOrWhiteSpace(command.NewPassword))
            {
                var user = await userService.GetByIdAsync(command.Id, ct);
                if (user != null)
                {
                    var passResult = await identityService.AdminChangePasswordAsync(user.IdentityId, command.NewPassword, ct);
                    if (!passResult.IsSuccess)
                    {

                        return Result<bool>.Success(true, "اطلاعات ویرایش شد اما تغییر رمز عبور با خطا مواجه شد: " + passResult.Message);
                    }
                }
            }

            return Result<bool>.Success(true, "اطلاعات کاربر با موفقیت ویرایش شد.");
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"خطای سیستمی: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ChangePasswordAsync(ChangePasswordDto command, CancellationToken ct)
    {
        var user = await userService.GetByIdAsync(command.UserId, ct);
        if (user == null)
            return Result<bool>.Failure("کاربر یافت نشد.");

        var result = await identityService.ChangePasswordAsync(
            user.IdentityId,
            command.CurrentPassword,
            command.NewPassword,
            ct
        );

        return result;
    }

    public async Task<Result<bool>> UpdateEmailAsync(int userId, string newEmail, CancellationToken ct)
    {
        var user = await userService.GetByIdAsync(userId, ct);
        if (user == null) return Result<bool>.Failure("کاربر یافت نشد.");

        if (await userService.IsEmailExistAsync(newEmail, ct))
        {
            var currentEmail = await identityService.GetEmailByIdentityIdAsync(user.IdentityId, ct);
            if (currentEmail != newEmail)
                return Result<bool>.Failure("این ایمیل قبلاً استفاده شده است.");
        }

        var result = await identityService.ChangeEmailAsync(user.IdentityId, newEmail, ct);
        if (!result.IsSuccess) return result;

        var updateUserDto = new UpdateUserDto
        {
            Id = userId,
            Email = newEmail
        };
        await userService.UpdateAsync(updateUserDto, ct);

        return Result<bool>.Success(true, "ایمیل با موفقیت تغییر یافت.");
    }

    public async Task<Result<LoginResultDto>> LoginWithGoogleAsync(LoginWithGoogleDto command, CancellationToken ct)
    {
        var result = await identityService.LoginWithGoogleAsync(command, ct);
        if (!result.Succeeded)
            return Result<LoginResultDto>.Failure(result.Message ?? "خطا در ورود با گوگل.");
        return Result<LoginResultDto>.Success(result);
    }

    public async Task<Result<UserDetailDto>> GetUserProfileAsync(int userId, CancellationToken ct)
    {
        var user = await userService.GetByIdAsync(userId, ct);
        if (user == null) return Result<UserDetailDto>.Failure("کاربر یافت نشد.", "404");
        return Result<UserDetailDto>.Success(user);
    }

    public async Task<Result<bool>> EditUserProfileAsync(UpdateUserDto command, CancellationToken ct)
    {
        var isUpdated = await userService.UpdateAsync(command, ct);
        if (!isUpdated) return Result<bool>.Failure("ویرایش انجام نشد یا کاربر وجود ندارد.");
        return Result<bool>.Success(true, "اطلاعات با موفقیت ویرایش شد.");
    }

    public async Task<Result<bool>> ChangeUserBalanceAsync(int userId, decimal amount, CancellationToken ct)
    {
        var result = await userService.ChangeBalanceAsync(userId, amount, ct);
        if (!result) return Result<bool>.Failure("خطا در تغییر موجودی.");
        return Result<bool>.Success(true, amount > 0 ? "شارژ انجام شد." : "برداشت انجام شد.");
    }

    public async Task<Result<List<UserSummaryDto>>> GetUsersListAsync(PaginationRequestDto search, CancellationToken ct)
    {
        var users = await userService.GetAllAsync(search, ct);
        return Result<List<UserSummaryDto>>.Success(users);
    }

    public async Task<Result<bool>> DeleteUserAsync(int userId, CancellationToken ct)
    {
        var user = await userService.GetByIdAsync(userId, ct);
        if (user == null) return Result<bool>.Failure("کاربر یافت نشد.");

        try
        {
            var identityResult = await identityService.DeactivateUserAsync(user.IdentityId, ct);
            if (!identityResult.IsSuccess)
            {
                return Result<bool>.Failure($"خطا در حذف اکانت سیستمی: {identityResult.Message}");
            }

            var dbResult = await userService.DeleteAsync(userId, ct);

            if (!dbResult)
                return Result<bool>.Failure("کاربر در دیتابیس اصلی یافت نشد اما اکانت سیستمی غیرفعال شد.");

            return Result<bool>.Success(true, "کاربر با موفقیت حذف و اطلاعات تماس او آزاد شد.");
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"خطای سیستمی: {ex.Message}");
        }
    }
}