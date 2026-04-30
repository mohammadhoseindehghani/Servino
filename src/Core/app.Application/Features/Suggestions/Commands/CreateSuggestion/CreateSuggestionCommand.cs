using app.Application.Common;
using app.Application.DTOs.SuggestionDTOs;
using MediatR;

namespace app.Application.Features.Suggestions.Commands.CreateSuggestion;

public record CreateSuggestionCommand(CreateSuggestionDto Command)
    : IRequest<Result<bool>>;
