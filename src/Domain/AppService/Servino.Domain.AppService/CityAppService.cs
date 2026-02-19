using Microsoft.Extensions.Logging;
using Servino.Domain.Core._common;
using Servino.Domain.Core.LocationAgg.Contracts.AppService;
using Servino.Domain.Core.LocationAgg.Contracts.Service;
using Servino.Domain.Core.LocationAgg.Dtos;
using Servino.Framework.Caching;

namespace Servino.Domain.AppService;

public class CityAppService(ICityService cityService, ICacheService cache, ILogger<CityAppService> logger) : ICityAppService
{
    public async Task<Result<bool>> CreateAsync(string title, int provinceId, CancellationToken ct)
    {
        var result = await cityService.CreateAsync(title, provinceId, ct);

        if (result)
        {
            await cache.RemoveAsync(CacheKeys.CitiesAll("", 1, 10), ct); 
            await cache.RemoveAsync(CacheKeys.CitiesByProvinceId(provinceId), ct); 
        }

        return !result ? Result<bool>.Failure("عملیات ایجاد با شکست مواجه شد") : Result<bool>.Success(result);
    }

    public async Task<Result<bool>> UpdateAsync(int id, string title, int provinceId, CancellationToken ct)
    {
        var result = await cityService.UpdateAsync(id, title, provinceId, ct);

        if (result)
        {
            await cache.RemoveAsync(CacheKeys.CityDetails(id), ct); 
            await cache.RemoveAsync(CacheKeys.CitiesByProvinceId(provinceId), ct); 
        }

        return !result ? Result<bool>.Failure("عملیات اپدیت با شکست مواجه شد") : Result<bool>.Success(result);
    }

    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken ct)
    {
        var result = await cityService.DeleteAsync(id, ct);

        if (result)
        {
            var city = await cityService.GetByIdAsync(id, ct);
            if (city != null)
            {
                await cache.RemoveAsync(CacheKeys.CityDetails(id), ct); 
                await cache.RemoveAsync(CacheKeys.CitiesByProvinceId(city.ProvinceId), ct);
            }
        }

        return !result ? Result<bool>.Failure("عملیات حذف با شکست مواجه شد") : Result<bool>.Success(result);
    }

    public async Task<Result<CityDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        try
        {
            var key = CacheKeys.CityDetails(id);
            var city = await cache.GetOrSetAsync(
                key,
                async () => await cityService.GetByIdAsync(id, ct),
                CacheTtl.CityDetails,
                ct);

            return city == null
                ? Result<CityDto>.Failure("شهری یافت نشد.")
                : Result<CityDto>.Success(city);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "System error in CityAppService.GetByIdAsync | CityId: {CityId}", id);
            return Result<CityDto>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }


    public async Task<List<CityDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        var key = CacheKeys.CitiesAll(search.SearchKey ?? "", search.PageNumber, search.PageSize);

        return await cache.GetOrSetAsync(key,
            async () => await cityService.GetAllAsync(search, ct), CacheTtl.Cities, ct);
    }


    public async Task<int> GetCountAsync(CancellationToken ct)
    {
        return await cityService.GetCountAsync(ct);
    }

    public async Task<List<SelectListDto>> GetCitiesByProvinceIdAsync(int provinceId, CancellationToken ct)
    {
        var key = CacheKeys.CitiesByProvinceId(provinceId);

        return await cache.GetOrSetAsync(key,
            async () => await cityService.GetCitiesByProvinceIdAsync(provinceId, ct), CacheTtl.Cities, ct);
    }








    private static class CacheKeys
    {
        public static string CityDetails(int id) => $"city:details:{id}";
        public static string CitiesAll(string searchKey, int pageNumber, int pageSize) =>
            $"cities:all:{searchKey}:{pageNumber}:{pageSize}";
        public static string CitiesByProvinceId(int provinceId) => $"cities:province:{provinceId}";
    }

    private static class CacheTtl
    {
        public static readonly TimeSpan CityDetails = TimeSpan.FromMinutes(5);
        public static readonly TimeSpan Cities = TimeSpan.FromMinutes(10);
    }

}