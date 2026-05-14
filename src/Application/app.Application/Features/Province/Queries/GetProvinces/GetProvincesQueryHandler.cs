using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services;
using app.Application.Contracts.DTOs.LocationDTOs;
using MediatR;

namespace app.Application.Features.Province.Queries.GetProvinces;

public class GetProvincesQueryHandler(
    IProvinceRepository provinceRepository,
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
            async () => await provinceRepository.GetAllAsync(request.Search, ct),
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
