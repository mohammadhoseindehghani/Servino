using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Providers_Services;
using app.Application.Contracts.Contracts.Services.HomeServiceAgg;
using app.Application.Contracts.DTOs.HomeServiceDTOs;
using MediatR;

namespace app.Application.Features.HomeServices.Queries.GetHomeServices;

public class GetHomeServicesQueryHandler(
    IHomeServiceService homeServiceService,
    ICacheService cache)
    : IRequestHandler<GetHomeServicesQuery, Result<List<HomeServiceSummaryDto>>>
{

    public async Task<Result<List<HomeServiceSummaryDto>>> Handle(GetHomeServicesQuery request, CancellationToken ct)
    {
        var key = CacheKeys.HomeServicesAll(
            request.Search.SearchKey ?? "",
            request.Search.PageNumber,
            request.Search.PageSize);

        return await cache.GetOrSetAsync(
            key,
            async () => await homeServiceService.GetAllAsync(request.Search, ct),
            CacheTtl.HomeServices,
            ct);
    }
    private static class CacheKeys
    {
        public static string HomeServicesAll(string searchKey, int pageNumber, int pageSize) =>
            $"homeServices:all:{searchKey}:{pageNumber}:{pageSize}";
    }

    private static class CacheTtl
    {
        public static readonly TimeSpan HomeServices = TimeSpan.FromMinutes(10);
    }
}
