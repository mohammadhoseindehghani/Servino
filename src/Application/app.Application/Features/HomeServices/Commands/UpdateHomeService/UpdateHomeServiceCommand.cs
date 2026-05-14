using app.Application.Contracts.Common;
using MediatR;

namespace app.Application.Features.HomeServices.Commands.UpdateHomeService;

public record UpdateHomeServiceCommand(
    int Id,
    string Title,
    decimal BasePrice,
    int CategoryId
) : IRequest<Result<bool>>;
