using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Providers_Services;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.DTOs.LocationDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.City.Queries.GetCityById;

public class GetCityByIdQueryHandler(
    ICityRepository cityRepository,
    ICacheService cache,
    ILogger<GetCityByIdQueryHandler> logger)
    : IRequestHandler<GetCityByIdQuery, Result<CityDto>>
{

    public async Task<Result<CityDto>> Handle(GetCityByIdQuery request, CancellationToken ct)
    {
        try
        {
            var key = CacheKeys.CityDetails(request.Id);

            var city = await cache.GetOrSetAsync(
                key,
                async () => await cityRepository.GetByIdAsync(request.Id, ct),
                CacheTtl.CityDetails,
                ct);

            return city == null
                ? Result<CityDto>.Failure("شهری یافت نشد.")
                : Result<CityDto>.Success(city);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "System error in GetCityByIdQueryHandler | CityId: {CityId}", request.Id);
            return Result<CityDto>.Failure("خطای سیستمی رخ داده است.");
        }
    }
    private static class CacheKeys
    {
        public static string CityDetails(int id) => $"city:details:{id}";
    }

    private static class CacheTtl
    {
        public static readonly TimeSpan CityDetails = TimeSpan.FromMinutes(5);
    }
}
