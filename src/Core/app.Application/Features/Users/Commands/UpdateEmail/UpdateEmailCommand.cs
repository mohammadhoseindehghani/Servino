using app.Application.Common;
using MediatR;

namespace app.Application.Features.Users.Commands.UpdateEmail;

public record UpdateEmailCommand(int UserId, string NewEmail)
    : IRequest<Result<bool>>;
