using app.Application.Contracts.Repositories;
using app.Application.Contracts.Services;
using app.Application.DTOs.CategoryDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Categories.Queries.GetBreadcrumb;

public class GetCategoryBreadcrumbHandler(
    ICategoryRepository categoryRepository,
    ICacheService cache,
    ILogger<GetCategoryBreadcrumbHandler> logger) 
    : IRequestHandler<GetCategoryBreadcrumbQuery, List<BreadcrumbDto>>
{
    public async Task<List<BreadcrumbDto>> Handle(GetCategoryBreadcrumbQuery request,
        CancellationToken ct)
    {
        var key = CacheKeys.Breadcrumb(request.CategoryId);

        return await cache.GetOrSetAsync(
            key, async () => await categoryRepository.GetBreadcrumbAsync(request.CategoryId, ct),
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