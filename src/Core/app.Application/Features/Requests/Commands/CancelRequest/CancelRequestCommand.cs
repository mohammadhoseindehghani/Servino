using app.Application.Common;
using MediatR;

namespace app.Application.Features.Requests.Commands.CancelRequest;

public record CancelRequestCommand(
    int RequestId,
    int CustomerId
) : IRequest<Result<bool>>;
