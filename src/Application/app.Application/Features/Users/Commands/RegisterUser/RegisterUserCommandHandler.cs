using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services;
using app.Application.Contracts.DTOs.UserDTOs;
using FluentValidation;
using MediatR;

namespace app.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IExpertRepository expertRepository,
    ICustomerRepository customerRepository,
    IIdentityService identityService,
    IValidator<RegisterUserCommand> validator)
    : IRequestHandler<RegisterUserCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var c = request.Command;
        if (await userRepository.IsEmailExistAsync(c.Email, ct))
            return Result<bool>.Failure("این ایمیل قبلاً در سیستم ثبت شده است.");
        if (await userRepository.IsMobileExistAsync(c.PhoneNumber, ct))
            return Result<bool>.Failure("این شماره قبلاً در سیستم ثبت شده است.");
        if (c.Password.Length < 6)
            return Result<bool>.Failure("پسورد باید حداقل ۶ کاراکتر داشته باشد.");

        string? identityId = null;
        int userId = 0;

        try
        {
            var identityResult = await identityService.RegisterWithEmailAsync(c, ct);
            if (!identityResult.Succeeded)
                return Result<bool>.Failure(identityResult.Message ?? "خطا در ثبت نام امنیتی.");

            identityId = identityResult.Id!;
            var createUserDto = new CreateUserDto
            {
                FirstName = "کاربر",
                LastName = "جدید",
                Email = c.Email,
                Mobile = c.PhoneNumber,
                IdentityId = identityId
            };

            if (!await userRepository.CreateAsync(createUserDto, ct))
                throw new Exception("خطا در ذخیره اطلاعات کاربر.");

            userId = await userRepository.GetIdByIdentityIdAsync(identityId, ct);
            bool roleCreated = c.Role switch
            {
                "Customer" => await customerRepository.CreateAsync(userId, ct),
                "Expert" => await expertRepository.CreateAsync(userId, ct),
                _ => throw new Exception("نقش کاربر نامعتبر است.")
            };

            if (!roleCreated)
                throw new Exception("خطا در ایجاد اطلاعات نقش.");

            return Result<bool>.Success(true, "ثبت نام با موفقیت انجام شد.");
        }
        catch
        {
            if (userId > 0)
            {
                if (c.Role == "Customer") await customerRepository.HardDeleteByUserIdAsync(userId, ct);
                if (c.Role == "Expert") await expertRepository.HardDeleteByUserIdAsync(userId, ct);
                await userRepository.HardDeleteAsync(userId, ct);
            }

            if (!string.IsNullOrWhiteSpace(identityId))
                await identityService.DeleteUserAsync(identityId, ct);

            return Result<bool>.Failure("خطای غیرمنتظره در ثبت نام.");
        }
    }
}
