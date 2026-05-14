using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Providers_Services;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.UserAgg;
using app.Application.Contracts.DTOs.IdentityDTOs;
using app.Application.Contracts.DTOs.UserDTOs;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Users.Commands.CreateUserByAdmin;

public class CreateUserByAdminCommandHandler(
    IUserService userService,
    ICustomerRepository customerRepository,
    IExpertRepository expertRepository,
    IIdentityService identityService,
    ILogger<CreateUserByAdminCommandHandler> logger,
    IValidator<CreateUserByAdminCommand> validator)
    : IRequestHandler<CreateUserByAdminCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateUserByAdminCommand request, CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var c = request.Command;

        if (await userService.IsEmailExistAsync(c.Email, ct))
            return Result<bool>.Failure("این ایمیل قبلاً ثبت شده است.");
        if (await userService.IsMobileExistAsync(c.Mobile, ct))
            return Result<bool>.Failure("این شماره موبایل قبلاً ثبت شده است.");

        string? identityId = null;
        int userId = 0;

        try
        {
            var identityResult = await identityService.RegisterWithEmailAsync(
                new RegisterDto { Email = c.Email, Password = c.Password, PhoneNumber = c.Mobile },
                c.Role, ct);

            if (!identityResult.Succeeded)
                return Result<bool>.Failure(identityResult.Message ?? "خطا در سیستم هویت‌سنجی.");

            identityId = identityResult.Id!;
            var createUserDto = new CreateUserDto
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Mobile = c.Mobile,
                IdentityId = identityId,
                CityId = 1
            };

            var ok = await userService.CreateAsync(createUserDto, ct);
            if (!ok) throw new Exception("خطا در ذخیره اطلاعات کاربر.");

            userId = await userService.GetIdByIdentityIdAsync(identityId, ct);

            bool roleResult = c.Role switch
            {
                "Expert" => await expertRepository.CreateAsync(userId, ct),
                "Customer" => await customerRepository.CreateAsync(userId, ct),
                "Admin" => true,
                _ => throw new Exception("نقش نامعتبر است.")
            };

            if (!roleResult)
                throw new Exception("خطا در ایجاد اطلاعات نقش کاربر.");

            return Result<bool>.Success(true, "کاربر با موفقیت ایجاد شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating user by admin {Email}", c.Email);

            if (userId > 0)
            {
                if (c.Role == "Customer") await customerRepository.HardDeleteByUserIdAsync(userId, ct);
                if (c.Role == "Expert") await expertRepository.HardDeleteByUserIdAsync(userId, ct);
                await userService.HardDeleteAsync(userId, ct);
            }

            if (!string.IsNullOrWhiteSpace(identityId))
                await identityService.DeleteUserAsync(identityId, ct);

            return Result<bool>.Failure("خطای سیستمی در ایجاد کاربر. لطفاً مجدداً تلاش کنید.");
        }
    }
}
