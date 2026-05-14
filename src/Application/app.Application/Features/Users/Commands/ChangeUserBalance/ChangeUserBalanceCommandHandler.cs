using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.UserAgg;
using FluentValidation;
using MediatR;

namespace app.Application.Features.Users.Commands.ChangeUserBalance;

public class ChangeUserBalanceCommandHandler(IUserService userService, IValidator<ChangeUserBalanceCommand> validator)
    : IRequestHandler<ChangeUserBalanceCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(ChangeUserBalanceCommand request, CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var result = await userService.ChangeBalanceAsync(request.UserId, request.Amount, ct);
        return !result ? Result<bool>.Failure("خطا در تغییر موجودی.")
            : Result<bool>.Success(true, request.Amount > 0 ? "شارژ انجام شد." : "برداشت انجام شد.");
    }
}
