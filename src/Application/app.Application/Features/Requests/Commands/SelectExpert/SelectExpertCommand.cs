using app.Application.Contracts.Common;
using MediatR;

namespace app.Application.Features.Requests.Commands.SelectExpert;

public record SelectExpertCommand(
    int RequestId,
    int SuggestionId,
    int CustomerId
) : IRequest<Result<bool>>;
