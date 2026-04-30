using app.Application.Common;
using MediatR;

namespace app.Application.Features.Users.Commands.DeleteUserCommand;

public record DeleteUserCommand(int UserId)
    : IRequest<Result<bool>>;
