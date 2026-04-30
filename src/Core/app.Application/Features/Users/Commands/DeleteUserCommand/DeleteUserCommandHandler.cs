using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.Contracts.Services;
using MediatR;

namespace app.Application.Features.Users.Commands.DeleteUserCommand;

public class DeleteUserCommandHandler(
    IUserRepository userRepository,
    IExpertRepository expertRepository,
    ICustomerRepository customerRepository,
    IIdentityService identityService)
    : IRequestHandler<DeleteUserCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken ct)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, ct);
        if (user is null)
            return Result<bool>.Failure("کاربر یافت نشد.");

        await expertRepository.HardDeleteByUserIdAsync(user.Id, ct);
        await customerRepository.HardDeleteByUserIdAsync(user.Id, ct);

        var deleted = await userRepository.HardDeleteAsync(user.Id, ct);
        if (!deleted)
            return Result<bool>.Failure("حذف کاربر با خطا مواجه شد.");

        if (!string.IsNullOrWhiteSpace(user.IdentityId))
            await identityService.DeleteUserAsync(user.IdentityId!, ct);

        return Result<bool>.Success(true, "کاربر با موفقیت حذف شد.");
    }
}
