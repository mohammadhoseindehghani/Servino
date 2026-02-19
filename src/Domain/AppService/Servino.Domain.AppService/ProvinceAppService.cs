using Microsoft.Extensions.Logging;
using Servino.Domain.Core._common;
using Servino.Domain.Core.LocationAgg.Contracts.AppService;
using Servino.Domain.Core.LocationAgg.Contracts.Service;
using Servino.Domain.Core.LocationAgg.Dtos;
using Servino.Framework.Caching;

namespace Servino.Domain.AppService;

public class ProvinceAppService(IProvinceService provinceService, ICacheService cache, ILogger<ProvinceAppService> logger) : IProvinceAppService
{
    public async Task<Result<bool>> CreateAsync(string title, CancellationToken ct)
    {
        var result = await provinceService.CreateAsync(title, ct);

        if (result)
        {
            await cache.RemoveAsync(CacheKeys.ProvincesAll("", 1, 10), ct);
            await cache.RemoveAsync(CacheKeys.ProvincesForDropdown, ct);
        }

        return !result ? Result<bool>.Failure("استان اینجاد نشد") : Result<bool>.Success(result);
    }

    public async Task<Result<bool>> UpdateAsync(int id, string title, CancellationToken ct)
    {
        var result = await provinceService.UpdateAsync(id, title, ct);

        if (result)
        {
            await cache.RemoveAsync(CacheKeys.ProvinceDetails(id), ct);
            await cache.RemoveAsync(CacheKeys.ProvincesForDropdown, ct);
        }

        return !result ? Result<bool>.Failure("اپدیت استان با شکست مواجه شد.") : Result<bool>.Success(result);
    }

    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken ct)
    {
        var result = await provinceService.DeleteAsync(id, ct);

        if (result)
        {
            await cache.RemoveAsync(CacheKeys.ProvinceDetails(id), ct);
            await cache.RemoveAsync(CacheKeys.ProvincesForDropdown, ct);
        }

        return !result ? Result<bool>.Failure("حذف با شکست مواجه شد") : Result<bool>.Success(result);
    }

    public async Task<Result<ProvinceDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        try
        {
            var key = CacheKeys.ProvinceDetails(id);
            var province = await cache.GetOrSetAsync(key,
                async () => await provinceService.GetByIdAsync(id, ct), CacheTtl.ProvinceDetails, ct);

            return province == null
                ? Result<ProvinceDto>.Failure("استانی یافت نشد")
                : Result<ProvinceDto>.Success(province);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "System error in ProvinceAppService.GetByIdAsync | ProvinceId: {ProvinceId}", id);
            return Result<ProvinceDto>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }

    public async Task<List<ProvinceDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        var key = CacheKeys.ProvincesAll(search.SearchKey ?? "", search.PageNumber, search.PageSize);

        return await cache.GetOrSetAsync(key,
            async () => await provinceService.GetAllAsync(search, ct),
            CacheTtl.Provinces, ct);
    }


    public async Task<int> GetCountAsync(CancellationToken ct)
    {
        return await provinceService.GetCountAsync(ct);
    }

    public async Task<List<SelectListDto>> GetAllForDropdownAsync(CancellationToken ct)
    {
        var key = CacheKeys.ProvincesForDropdown;

        return await cache.GetOrSetAsync(key,
            async () => await provinceService.GetAllForDropdownAsync(ct), CacheTtl.Provinces, ct);
    }




    private static class CacheKeys
    {
        public static string ProvinceDetails(int id) => $"province:details:{id}";
        public static string ProvincesAll(string searchKey, int pageNumber, int pageSize) =>
            $"provinces:all:{searchKey}:{pageNumber}:{pageSize}";
        public static string ProvincesForDropdown => "provinces:dropdown";
    }

    private static class CacheTtl
    {
        public static readonly TimeSpan ProvinceDetails = TimeSpan.FromMinutes(5);
        public static readonly TimeSpan Provinces = TimeSpan.FromMinutes(10);
    }

}