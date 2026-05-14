using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.Suggestion;
using app.Application.Contracts.DTOs.SuggestionDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Suggestions.Queries.GetSuggestionById;

public class GetSuggestionByIdQueryHandler(
    ISuggestionService suggestionService,
    ILogger<GetSuggestionByIdQueryHandler> logger)
    : IRequestHandler<GetSuggestionByIdQuery, Result<SuggestionDto>>
{
    public async Task<Result<SuggestionDto>> Handle(
        GetSuggestionByIdQuery query,
        CancellationToken ct)
    {
        try
        {
            var suggestion = await suggestionService.GetByIdAsync(query.Id, ct);

            return suggestion == null
                ? Result<SuggestionDto>.Failure("پیشنهاد یافت نشد.")
                : Result<SuggestionDto>.Success(suggestion);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "System error in SuggestionAppService.GetByIdAsync | SuggestionId: {SuggestionId}",
                query.Id);

            return Result<SuggestionDto>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }
}
