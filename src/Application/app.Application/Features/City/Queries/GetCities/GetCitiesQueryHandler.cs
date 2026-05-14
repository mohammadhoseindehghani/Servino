using app.Application.Contracts.Contracts.Providers_Services;
using app.Application.Contracts.Contracts.Services.CityAgg;
using app.Application.Contracts.DTOs.LocationDTOs;
using MediatR;

namespace app.Application.Features.City.Queries.GetCities;

public class GetCitiesQueryHandler(ICityService cityService, ICacheService cache)
    : IRequestHandler<GetCitiesQuery, List<CityDto>>
{

    public async Task<List<CityDto>> Handle(GetCitiesQuery request, CancellationToken ct)
    {
        var key = CacheKeys.CitiesAll(
            request.Search.SearchKey ?? "",
            request.Search.PageNumber,
            request.Search.PageSize);

        return await cache.GetOrSetAsync(
            key,
            async () => await cityService.GetAllAsync(request.Search, ct),
            CacheTtl.Cities,
            ct);
    }
    private static class CacheKeys
    {
        public static string CitiesAll(string searchKey, int pageNumber, int pageSize) =>
            $"cities:all:{searchKey}:{pageNumber}:{pageSize}";
    }

    private static class CacheTtl
    {
        public static readonly TimeSpan Cities = TimeSpan.FromMinutes(10);
    }
}
