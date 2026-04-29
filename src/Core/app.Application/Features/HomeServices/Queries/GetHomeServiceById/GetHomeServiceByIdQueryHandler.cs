using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.Contracts.Services;
using app.Application.DTOs.HomeServiceDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.HomeServices.Queries.GetHomeServiceById;

public class GetHomeServiceByIdQueryHandler(
    IHomeServiceRepository homeServiceRepository,
    ICacheService cache,
    ILogger<GetHomeServiceByIdQueryHandler> logger)
    : IRequestHandler<GetHomeServiceByIdQuery, Result<HomeServiceDto>>
{

    public async Task<Result<HomeServiceDto>> Handle(GetHomeServiceByIdQuery request, CancellationToken ct)
    {
        try
        {
            var key = CacheKeys.HomeServiceDetails(request.Id);

            var service = await cache.GetOrSetAsync(
                key,
                async () => await homeServiceRepository.GetByIdAsync(request.Id, ct),
                CacheTtl.HomeServiceDetails,
                ct);

            return service == null
                ? Result<HomeServiceDto>.Failure("خدمت مورد نظر یافت نشد.", "404")
                : Result<HomeServiceDto>.Success(service);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error retrieving HomeService | Id: {Id}",
                request.Id);

            return Result<HomeServiceDto>.Failure("خطای سیستمی رخ داده است.");
        }
    }
    private static class CacheKeys
    {
        public static string HomeServiceDetails(int id) => $"homeService:details:{id}";
    }

    private static class CacheTtl
    {
        public static readonly TimeSpan HomeServiceDetails = TimeSpan.FromMinutes(5);
    }
}
