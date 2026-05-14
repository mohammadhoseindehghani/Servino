using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services;
using app.Application.Contracts.DTOs.CategoryDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler(
    ICategoryRepository categoryRepository,
    ICacheService cache,
    ILogger<GetCategoriesQueryHandler> logger) 
    : IRequestHandler<GetCategoriesQuery, List<CategorySummaryDto>>
{
    public async Task<List<CategorySummaryDto>> Handle(GetCategoriesQuery request, CancellationToken ct)
    {
        var key = CacheKeys.CategoriesAll(request.SearchKey ?? "", request.PageNumber, request.PageSize);
        var dto = new PaginationRequestDto()
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            SearchKey = request.SearchKey
        };
        return await cache.GetOrSetAsync(
            key,
            async () => await categoryRepository.GetAllAsync(dto, ct), CacheTtl.Categories, ct);
    }
    private static class CacheKeys
    { 
        public static string CategoriesAll(string searchKey, int pageNumber, int pageSize) =>
            $"categories:all:{searchKey}:{pageNumber}:{pageSize}";
    }
    private static class CacheTtl
    {
        public static readonly TimeSpan Categories = TimeSpan.FromMinutes(10);
    }
}