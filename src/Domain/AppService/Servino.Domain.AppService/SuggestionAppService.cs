using Microsoft.Extensions.Logging;
using Servino.Domain.Core.SuggestionAgg.Contracts.AppService;
using Servino.Domain.Core.SuggestionAgg.Contracts.Service;
using Servino.Domain.Core.SuggestionAgg.Dtos;
using Servino.Framework.Caching;

namespace Servino.Domain.AppService;

public class SuggestionAppService(ISuggestionService suggestionService, ICacheService cache,
    ILogger<SuggestionAppService> logger) : ISuggestionAppService
{
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