using app.Application.Common;
using app.Application.DTOs.SuggestionDTOs;
using MediatR;

namespace app.Application.Features.Suggestions.Queries.GetSuggestionById;

public record GetSuggestionByIdQuery(int Id)
    : IRequest<Result<SuggestionDto>>;
