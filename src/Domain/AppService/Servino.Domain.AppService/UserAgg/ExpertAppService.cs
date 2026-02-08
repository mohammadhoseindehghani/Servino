using Microsoft.Extensions.Logging;
using Servino.Domain.Core._common;
using Servino.Domain.Core.ExpertHomeServiceAgg.Contracts.Service;
using Servino.Domain.Core.HomeServiceAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Dtos;
using Servino.Framework.Caching;

namespace Servino.Domain.AppService.UserAgg;

public class ExpertAppService(
    IExpertService expertService,                   
    IHomeServiceService homeServiceService,        
    IExpertHomeServiceService expertHomeServiceService,
    ICacheService cache, ILogger<ExpertAppService> logger
) : IExpertAppService
{
    public async Task<Result<ExpertProfileDto>> GetByUserId(int userId, CancellationToken ct)
    {
        try
        {
            var key = CacheKeys.ExpertProfile(userId);

            var profile = await cache.GetOrSetAsync(key,
                async () => await expertService.GetByUserId(userId, ct), CacheTtl.ExpertProfile, ct);

            return profile == null
                ? Result<ExpertProfileDto>.Failure("پروفایلی یافت نشد")
                : Result<ExpertProfileDto>.Success(profile);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "System error in ExpertAppService.GetByUserId | UserId: {UserId}", userId);
            return Result<ExpertProfileDto>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }

    public async Task<Result<ExpertProfileDto>> GetByExpertId(int expertId, CancellationToken ct)
    {
        var result = await expertService.GetByExpertIdAsync(expertId, ct);
        return result is null ? Result<ExpertProfileDto>.Failure("مشخصات متخصص یافت نشد") 
            : Result<ExpertProfileDto>.Success(result);
    }

    public async Task<Result<bool>> UpdateProfile(UpdateExpertProfileDto command, CancellationToken ct)
    {
        var result = await expertService.UpdateProfile(command, ct);

        if (result)
        {
            await cache.RemoveAsync(CacheKeys.ExpertProfile(command.UserId), ct);
        }

        return !result ? Result<bool>.Failure("عملیات اپدیت با شکست مواجه شد")
            : Result<bool>.Success(true, "اپدیت اطلاعات با موفقیت انجام شد");
    }


    public async Task<Result<List<ExpertServiceItemDto>>> GetServicesForEditAsync(int userId, CancellationToken ct)
    {
        var expertId = await expertService.GetExpertIdByUserIdAsync(userId, ct);
        if (expertId == 0) return Result<List<ExpertServiceItemDto>>.Failure("اکسپرت یافت نشد.");

        var key = CacheKeys.ExpertServices(expertId);

        var services = await cache.GetOrSetAsync(key,
            async () =>
            {
                var allServices = await homeServiceService.GetAllActiveServicesAsync(ct);
                var myServiceIds = await expertHomeServiceService.GetSelectedServiceIdsAsync(expertId, ct);

                return allServices.Select(s => new ExpertServiceItemDto
                {
                    HomeServiceId = s.Id,
                    HomeServiceTitle = s.Title,
                    IsSelected = myServiceIds.Contains(s.Id)
                }).ToList();
            },
            CacheTtl.ExpertServices, ct);

        return Result<List<ExpertServiceItemDto>>.Success(services);
    }


    public async Task<Result<bool>> UpdateServicesAsync(int userId, List<int> selectedIds, CancellationToken ct)
    {
        var expertId = await expertService.GetExpertIdByUserIdAsync(userId, ct);
        if (expertId == 0) return Result<bool>.Failure("اکسپرت یافت نشد.");

        try
        {
            await expertHomeServiceService.UpdateExpertServicesAsync(expertId, selectedIds, ct);

            await cache.RemoveAsync(CacheKeys.ExpertServices(expertId), ct);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure("خطا: " + ex.Message);
        }
    }


    private static class CacheKeys
    {
        public static string ExpertProfile(int userId) => $"expert:profile:{userId}";
        public static string ExpertServices(int expertId) => $"expert:services:{expertId}";
    }

    private static class CacheTtl
    {
        public static readonly TimeSpan ExpertProfile = TimeSpan.FromMinutes(5);
        public static readonly TimeSpan ExpertServices = TimeSpan.FromMinutes(10);
    }

}
