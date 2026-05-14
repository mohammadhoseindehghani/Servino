using app.Application.Contracts.Common;
using MediatR;

namespace app.Application.Features.Requests.Commands.CreateRequest;

public record CreateRequestCommand(
    string Title,
    string Description,
    string Address,
    int CityId,
    int CustomerId,
    int HomeServiceId,
    DateTime DateRequired,
    List<string>? ImagePaths
) : IRequest<Result<int>>;
