using app.Application.Contracts.Common;
using MediatR;

namespace app.Application.Features.HomeServices.Commands.CreateHomeService;

public record CreateHomeServiceCommand(
    string Title,
    decimal BasePrice,
    int CategoryId
) : IRequest<Result<bool>>;
