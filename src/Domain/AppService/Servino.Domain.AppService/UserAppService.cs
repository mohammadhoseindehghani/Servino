using Servino.Domain.Core._common;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Dtos;
using Servino.Domain.Core.UserAgg.Dtos.Identity;

namespace Servino.Domain.AppService;

public class UserAppService(
        IIdentityService identityService,
        IUserService userService) : IUserAppService
{
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

        if (user == null)
            return Result<UserDetailDto>.Failure("کاربر یافت نشد.", "404");

        return Result<UserDetailDto>.Success(user);
    }

    public async Task<Result<bool>> EditUserProfileAsync(UpdateUserDto command, CancellationToken ct)
    {
        var isUpdated = await userService.UpdateAsync(command, ct);

        if (!isUpdated)
            return Result<bool>.Failure("ویرایش انجام نشد یا کاربر وجود ندارد.");

        return Result<bool>.Success(true, "اطلاعات با موفقیت ویرایش شد.");
    }

    public async Task<Result<bool>> ChangeUserBalanceAsync(int userId, decimal amount, CancellationToken ct)
    {
        var result = await userService.ChangeBalanceAsync(userId, amount, ct);

        if (!result)
            return Result<bool>.Failure("خطا در تغییر موجودی.");

        var msg = amount > 0 ? "شارژ انجام شد." : "برداشت انجام شد.";
        return Result<bool>.Success(true, msg);
    }

    public async Task<Result<List<UserSummaryDto>>> GetUsersListAsync(PaginationRequestDto search, CancellationToken ct)
    {
        var users = await userService.GetAllAsync(search, ct);
        return Result<List<UserSummaryDto>>.Success(users);
    }

    public async Task<Result<bool>> DeleteUserAsync(int userId, CancellationToken ct)
    {
        var dbResult = await userService.DeleteAsync(userId, ct);

        if (!dbResult)
            return Result<bool>.Failure("کاربر یافت نشد.");

        // var user = await userService.GetByIdAsync(userId, ct);
        // await identityService.ChangeAccountStatusAsync(user.IdentityId, lockAccount: true);

        return Result<bool>.Success(true, "کاربر با موفقیت حذف شد.");
    }
}