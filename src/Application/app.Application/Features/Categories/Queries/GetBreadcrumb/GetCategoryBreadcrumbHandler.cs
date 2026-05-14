using app.Application.Contracts.Contracts.Providers_Services;
using app.Application.Contracts.Contracts.Services.CategoryAgg;
using app.Application.Contracts.DTOs.CategoryDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Categories.Queries.GetBreadcrumb;

public class GetCategoryBreadcrumbHandler(
    ICategoryService categoryService,
    ICacheService cache,
    ILogger<GetCategoryBreadcrumbHandler> logger) 
    : IRequestHandler<GetCategoryBreadcrumbQuery, List<BreadcrumbDto>>
{
    public async Task<List<BreadcrumbDto>> Handle(GetCategoryBreadcrumbQuery request,
        CancellationToken ct)
    {
        var key = CacheKeys.Breadcrumb(request.CategoryId);

        return await cache.GetOrSetAsync(
            key, async () => await categoryService.GetBreadcrumbAsync(request.CategoryId, ct),
            CacheTtl.Breadcrumb, ct);
    }
    private static class CacheKeys
    {
        public static string Breadcrumb(int categoryId) => $"breadcrumb:{categoryId}";
    }
    private static class CacheTtl
    {
        public static readonly TimeSpan Breadcrumb = TimeSpan.FromMinutes(5);
    }
}