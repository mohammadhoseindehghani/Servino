using app.Application.Contracts.Contracts.Providers_Services;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.ProvinceAgg;
using app.Application.Contracts.DTOs.LocationDTOs;
using MediatR;

namespace app.Application.Features.Province.Queries.GetProvinces;

public class GetProvincesQueryHandler(
    IProvinceService provinceService,
    ICacheService cache) : IRequestHandler<GetProvincesQuery, List<ProvinceDto>>
{

    public async Task<List<ProvinceDto>> Handle(GetProvincesQuery request, CancellationToken ct)
    {
        var key = CacheKeys.ProvincesAll(
            request.Search.SearchKey ?? "",
            request.Search.PageNumber,
            request.Search.PageSize);

        return await cache.GetOrSetAsync(
            key,
            async () => await provinceService.GetAllAsync(request.Search, ct),
            CacheTtl.Provinces,
            ct);
    }
    private static class CacheKeys
    {
        public static string ProvincesAll(string searchKey, int pageNumber, int pageSize) =>
            $"provinces:all:{searchKey}:{pageNumber}:{pageSize}";
    }

    private static class CacheTtl
    {
        public static readonly TimeSpan Provinces = TimeSpan.FromMinutes(10);
    }
}
