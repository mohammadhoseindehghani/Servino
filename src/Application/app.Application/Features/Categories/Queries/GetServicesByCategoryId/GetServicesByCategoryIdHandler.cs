using app.Application.Contracts.Contracts.Providers_Services;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.CategoryAgg;
using app.Application.Contracts.DTOs.CategoryDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Categories.Queries.GetServicesByCategoryId;

public class GetServicesByCategoryIdHandler(
    ICategoryService categoryService,
    ICacheService cache,
    ILogger<GetServicesByCategoryIdHandler> logger)
: IRequestHandler<GetServicesByCategoryIdQuery, List<ServiceClientDto>>
{
    public async Task<List<ServiceClientDto>> Handle(GetServicesByCategoryIdQuery request, CancellationToken ct)
    {
        var key = CacheKeys.ServicesByCategoryId(request.CategoryId);

        var exists = await categoryService.IsCategoryExistAndActiveAsync(request.CategoryId, ct);
        if (!exists)
        {
            return [];
        }

        return await cache.GetOrSetAsync(
            key, async () => await categoryService.GetServicesByCategoryIdAsync(request.CategoryId, ct),
            CacheTtl.Services, ct);
    }

    private static class CacheKeys
    {
        public static string ServicesByCategoryId(int categoryId) => $"services:category:{categoryId}";
    }
    private static class CacheTtl
    {
        public static readonly TimeSpan Services = TimeSpan.FromMinutes(10);
    }
}