using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.SuggestionDTOs;
using MediatR;

namespace app.Application.Features.Suggestions.Queries.GetSuggestionById;

public record GetSuggestionByIdQuery(int Id)
    : IRequest<Result<SuggestionDto>>;
