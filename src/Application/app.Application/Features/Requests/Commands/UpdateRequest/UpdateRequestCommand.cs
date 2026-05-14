using app.Application.Contracts.Common;
using MediatR;

namespace app.Application.Features.Requests.Commands.UpdateRequest;

public record UpdateRequestCommand(
    int Id,
    string Title,
    string Description,
    string Address,
    int CityId,
    DateTime DateRequired
) : IRequest<Result<bool>>;
