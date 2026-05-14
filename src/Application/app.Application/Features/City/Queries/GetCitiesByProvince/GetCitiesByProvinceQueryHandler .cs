using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services;
using app.Application.Contracts.DTOs.LocationDTOs;
using MediatR;

namespace app.Application.Features.City.Queries.GetCitiesByProvince;

public class GetCitiesByProvinceQueryHandler(ICityRepository cityRepository, ICacheService cache)
    : IRequestHandler<GetCitiesByProvinceQuery, List<SelectListDto>>
{

    public async Task<List<SelectListDto>> Handle(GetCitiesByProvinceQuery request, CancellationToken ct)
    {
        var key = CacheKeys.CitiesByProvinceId(request.ProvinceId);

        return await cache.GetOrSetAsync(
            key,
            async () => await cityRepository.GetCitiesByProvinceIdAsync(request.ProvinceId, ct),
            CacheTtl.Cities,
            ct);
    }
    private static class CacheKeys
    {
        public static string CitiesByProvinceId(int provinceId) => $"cities:province:{provinceId}";
    }

    private static class CacheTtl
    {
        public static readonly TimeSpan Cities = TimeSpan.FromMinutes(10);
    }
}
