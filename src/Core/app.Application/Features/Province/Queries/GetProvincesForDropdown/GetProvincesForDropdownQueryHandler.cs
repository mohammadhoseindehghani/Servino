using app.Application.Contracts.Repositories;
using app.Application.Contracts.Services;
using app.Application.DTOs.LocationDTOs;
using MediatR;

namespace app.Application.Features.Province.Queries.GetProvincesForDropdown;

public class GetProvincesForDropdownQueryHandler(
    IProvinceRepository provinceRepository,
    ICacheService cache) : IRequestHandler<GetProvincesForDropdownQuery, List<SelectListDto>>
{

    public async Task<List<SelectListDto>> Handle(GetProvincesForDropdownQuery request, CancellationToken ct)
    {
        return await cache.GetOrSetAsync(
            CacheKeys.ProvincesForDropdown,
            () => provinceRepository.GetAllForDropdownAsync(ct),
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
