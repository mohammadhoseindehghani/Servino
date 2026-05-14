using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Providers_Services;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.UserAgg;
using FluentValidation;
using MediatR;

namespace app.Application.Features.Users.Commands.DeleteUserCommand;

public class DeleteUserCommandHandler(
    IUserService userService,
    IExpertRepository expertRepository,
    ICustomerRepository customerRepository,
    IIdentityService identityService,
    IValidator<DeleteUserCommand> validator)
    : IRequestHandler<DeleteUserCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var user = await userService.GetByIdAsync(request.UserId, ct);
        if (user is null)
            return Result<bool>.Failure("کاربر یافت نشد.");

        await expertRepository.HardDeleteByUserIdAsync(user.Id, ct);
        await customerRepository.HardDeleteByUserIdAsync(user.Id, ct);

        var deleted = await userService.HardDeleteAsync(user.Id, ct);
        if (!deleted)
            return Result<bool>.Failure("حذف کاربر با خطا مواجه شد.");

        if (!string.IsNullOrWhiteSpace(user.IdentityId))
            await identityService.DeleteUserAsync(user.IdentityId!, ct);

        return Result<bool>.Success(true, "کاربر با موفقیت حذف شد.");
    }
}
