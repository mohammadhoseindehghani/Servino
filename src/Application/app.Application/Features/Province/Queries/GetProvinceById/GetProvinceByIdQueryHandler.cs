using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services;
using app.Application.Contracts.DTOs.LocationDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Province.Queries.GetProvinceById;

public class GetProvinceByIdQueryHandler(
    IProvinceRepository provinceRepository,
    ICacheService cache,
    ILogger<GetProvinceByIdQueryHandler> logger)
    : IRequestHandler<GetProvinceByIdQuery, Result<ProvinceDto>>
{

    public async Task<Result<ProvinceDto>> Handle(GetProvinceByIdQuery request, CancellationToken ct)
    {
        try
        {
            var key = CacheKeys.ProvinceDetails(request.Id);

            var province = await cache.GetOrSetAsync(
                key,
                async () => await provinceRepository.GetByIdAsync(request.Id, ct),
                CacheTtl.ProvinceDetails,
                ct);

            return province == null
                ? Result<ProvinceDto>.Failure("استانی یافت نشد")
                : Result<ProvinceDto>.Success(province);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading province. Id={Id}", request.Id);
            return Result<ProvinceDto>.Failure("خطای سیستمی رخ داده است");
        }
    }
    private static class CacheKeys
    {
        public static string ProvinceDetails(int id) => $"province:details:{id}";
    }

    private static class CacheTtl
    {
        public static readonly TimeSpan ProvinceDetails = TimeSpan.FromMinutes(5);
    }
}
