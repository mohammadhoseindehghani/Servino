using Microsoft.Extensions.Logging;
using Servino.Domain.Core._common;
using Servino.Domain.Core.SuggestionAgg.Contracts.AppService;
using Servino.Domain.Core.SuggestionAgg.Contracts.Service;
using Servino.Domain.Core.SuggestionAgg.Dtos;
using Servino.Framework.Caching;

namespace Servino.Domain.AppService;

public class SuggestionAppService(ISuggestionService suggestionService, ICacheService cache,
    ILogger<SuggestionAppService> logger) : ISuggestionAppService
{
    public async Task<Result<bool>> CreateAsync(CreateSuggestionDto command, CancellationToken ct)
    {
        if (command.RequestId <= 0)
            return Result<bool>.Failure("درخواست نامعتبر است");

        if (command.ExpertId <= 0)
            return Result<bool>.Failure("متخصص امکان ارسال پیشنهاد را ندارد");

        if (command.SuggestedPrice < 100000)
            return Result<bool>.Failure("مبلغ پیشنهادی نمیتوند کمتر از 100 هزار تومان باشد.");


        if (command.SuggestedDate <= DateTime.Now)
            return Result<bool>.Failure("تاریخ پیشنهادی نمیتواند در گذشته باشد.");
        
        if (command.EstimatedDurationHours <= 0)
            return Result<bool>.Failure("طول ساعت کاری نامعتبر است.");


        var result = await suggestionService.CreateAsync(command, ct);
        return !result ? Result<bool>.Failure("ایجاد پیشنهاد با شکست مواجه شد.") 
            : Result<bool>.Success(result);
    }

    public async Task<List<SuggestionSummaryDto>> GetByRequestIdAsync(int requestId, CancellationToken ct)
    {
        try
        {
            var key = CacheKeys.SuggestionsByRequestId(requestId);
            return await cache.GetOrSetAsync(key,
                async () => await suggestionService.GetByRequestIdAsync(requestId, ct),
                CacheTtl.Suggestions, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "System error in SuggestionAppService.GetByRequestIdAsync | RequestId: {RequestId}", requestId);
            return new List<SuggestionSummaryDto>();  
        }
    }

    private static class CacheKeys
    {
        public static string SuggestionsByRequestId(int requestId) => $"suggestions:request:{requestId}";
    }

    private static class CacheTtl
    {
        public static readonly TimeSpan Suggestions = TimeSpan.FromMinutes(10);
    }

}