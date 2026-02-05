using Microsoft.Extensions.Logging;
using Servino.Domain.Core._common;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Dtos;
using Servino.Framework.Caching;

namespace Servino.Domain.AppService.UserAgg;

public class CustomerAppService(ICustomerService customerService,
    ICacheService cache, ILogger<CustomerAppService> logger) : ICustomerAppService
{
    public async Task<Result<CustomerProfileDto>> GetByUserIdAsync(int userId, CancellationToken ct)
    {
        try
        {
            var key = CacheKeys.CustomerProfile(userId);

            var profile = await cache.GetOrSetAsync(key,
                async () => await customerService.GetByUserIdAsync(userId, ct), CacheTtl.CustomerProfile, ct);

            return profile == null
                ? Result<CustomerProfileDto>.Failure("اطلاعات یافت نشد.")
                : Result<CustomerProfileDto>.Success(profile);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "System error in CustomerAppService.GetByUserIdAsync | UserId: {UserId}", userId);
            return Result<CustomerProfileDto>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }

    public async Task<Result<bool>> UpdateProfile(UpdateCustomerProfileDto command, CancellationToken ct)
    {
        var result = await customerService.UpdateProfile(command, ct);

        if (result)
        {
            await cache.RemoveAsync(CacheKeys.CustomerProfile(command.UserId), ct);
        }

        return !result ? Result<bool>.Failure("اپدیت انجام نشد") : Result<bool>.Success(result,"اپدیت با موفقیت انحام شد.");
    }



    private static class CacheKeys
    {
        public static string CustomerProfile(int userId) => $"customer:profile:{userId}";
    }

    private static class CacheTtl
    {
        public static readonly TimeSpan CustomerProfile = TimeSpan.FromMinutes(5);
    }

}