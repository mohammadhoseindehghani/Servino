using app.Application.Contracts.Contracts.Providers_Services;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.DTOs.CategoryDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Categories.Queries.GetCategoriesByParentId;

public class GetCategoriesByParentIdQueryHandler(
    ICategoryRepository categoryRepository,
    ICacheService cache,
    ILogger<GetCategoriesByParentIdQueryHandler> logger) 
    : IRequestHandler<GetCategoriesByParentIdQuery, List<CategoryClientDto>>
{
    public async Task<List<CategoryClientDto>> Handle(GetCategoriesByParentIdQuery request, CancellationToken ct)
    {
        var key = CacheKeys.CategoriesByParentId(request.ParentId ?? 0);

        return await cache.GetOrSetAsync(key, async () => await categoryRepository.GetCategoriesByParentIdAsync(request.ParentId, ct),
            CacheTtl.Categories, ct);
    }

    private static class CacheKeys
    {
        public static string CategoriesByParentId(int parentId) => $"categories:parent:{parentId}";
    }
    private static class CacheTtl
    {
        public static readonly TimeSpan Categories = TimeSpan.FromMinutes(10);
    }
}