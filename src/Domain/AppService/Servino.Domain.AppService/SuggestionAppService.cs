using Microsoft.Extensions.Logging;
using Servino.Domain.Core._common;
using Servino.Domain.Core.RequestAgg.Contracts.Service;
using Servino.Domain.Core.SuggestionAgg.Contracts.AppService;
using Servino.Domain.Core.SuggestionAgg.Contracts.Service;
using Servino.Domain.Core.SuggestionAgg.Dtos;
using Servino.Framework.Caching;

namespace Servino.Domain.AppService;

public class SuggestionAppService(ISuggestionService suggestionService,IRequestService requestService, ICacheService cache,
    ILogger<SuggestionAppService> logger) : ISuggestionAppService
{
    public async Task<Result<bool>> CreateAsync(CreateSuggestionDto command, CancellationToken ct)
    {
        if (command.RequestId <= 0)
            return Result<bool>.Failure("درخواست نامعتبر است");

        if (command.ExpertId <= 0)
            return Result<bool>.Failure("متخصص امکان ارسال پیشنهاد را ندارد");

        var check = await suggestionService.IsExpertSendSuggestionBeforeAsync(command.ExpertId, command.RequestId, ct);
        if (check)
            return Result<bool>.Failure("برای این درخواست قبلا پیشنهاد ارسال کردید.");

        var basePrice = await requestService.GetBasePriceByRequestIdAsync(command.RequestId, ct);
        if (command.SuggestedPrice < basePrice)
            return Result<bool>.Failure($"مبلغ پیشنهادی نمیتوند کمتر از مبلغ پایه: {basePrice} باشد.");

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

    public async Task<Result<SuggestionDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var result = await suggestionService.GetByIdAsync(id, ct);
        return result is null ? Result<SuggestionDto>.Failure("پیشنهاد یافت نشد.") 
            : Result<SuggestionDto>.Success(result);
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