using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Domain.UserAgg.Entities;
using MediatR;

namespace app.Application.Features.Users.Commands.ChangeUserBalance;

public class ChangeUserBalanceCommandHandler(IUserRepository userRepository)
    : IRequestHandler<ChangeUserBalanceCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(ChangeUserBalanceCommand request, CancellationToken ct)
    {
        var result = await userRepository.ChangeBalanceAsync(request.UserId, request.Amount, ct);
        return !result ? Result<bool>.Failure("خطا در تغییر موجودی.")
            : Result<bool>.Success(true, request.Amount > 0 ? "شارژ انجام شد." : "برداشت انجام شد.");
    }
}
