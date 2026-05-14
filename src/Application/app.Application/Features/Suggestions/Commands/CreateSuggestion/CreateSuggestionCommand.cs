using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.SuggestionDTOs;
using MediatR;

namespace app.Application.Features.Suggestions.Commands.CreateSuggestion;

public record CreateSuggestionCommand(CreateSuggestionDto Command)
    : IRequest<Result<bool>>;
