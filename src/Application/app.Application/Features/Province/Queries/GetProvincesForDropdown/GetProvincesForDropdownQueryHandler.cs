using app.Application.Contracts.Contracts.Providers_Services;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.ProvinceAgg;
using app.Application.Contracts.DTOs.LocationDTOs;
using MediatR;

namespace app.Application.Features.Province.Queries.GetProvincesForDropdown;

public class GetProvincesForDropdownQueryHandler(
    IProvinceService provinceService,
    ICacheService cache) : IRequestHandler<GetProvincesForDropdownQuery, List<SelectListDto>>
{

    public async Task<List<SelectListDto>> Handle(GetProvincesForDropdownQuery request, CancellationToken ct)
    {
        return await cache.GetOrSetAsync(
            CacheKeys.ProvincesForDropdown,
            () => provinceService.GetAllForDropdownAsync(ct),
            CacheTtl.Provinces,
            ct);
    }
    private static class CacheKeys
    {
        public static string ProvincesForDropdown => "provinces:dropdown";
    }

    private static class CacheTtl
    {
        public static readonly TimeSpan Provinces = TimeSpan.FromMinutes(10);
    }
}
