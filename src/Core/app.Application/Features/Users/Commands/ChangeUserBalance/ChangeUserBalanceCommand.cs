using app.Application.Common;
using MediatR;

namespace app.Application.Features.Users.Commands.ChangeUserBalance;

public record ChangeUserBalanceCommand(int UserId, decimal Amount)
    : IRequest<Result<bool>>;
