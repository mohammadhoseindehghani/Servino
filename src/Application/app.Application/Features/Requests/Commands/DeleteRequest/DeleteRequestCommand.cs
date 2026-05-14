using app.Application.Contracts.Common;
using MediatR;

namespace app.Application.Features.Requests.Commands.DeleteRequest;

public record DeleteRequestCommand(int Id)
    : IRequest<Result<bool>>;