using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Providers_Services;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.CategoryAgg;
using app.Application.Contracts.DTOs.CategoryDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler(
    ICategoryService categoryService,
    ICacheService cache,
    ILogger<GetCategoryByIdQueryHandler> logger
    ) : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto>>
{
    public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken ct)
    {
        try
        {
            var key = CacheKeys.CategoryDetails(request.Id);
            var category = await cache.GetOrSetAsync(
                key, async () => await categoryService.GetByIdAsync(request.Id, ct),
                CacheTtl.CategoryDetails, ct);

            return category == null
                ? Result<CategoryDto>.Failure("دسته‌بندی مورد نظر یافت نشد.", "404")
                : Result<CategoryDto>.Success(category);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "System error in CategoryAppService.GetByIdAsync | CategoryId: {CategoryId}", request.Id);
            return Result<CategoryDto>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }
    private static class CacheKeys
    {
        public static string CategoryDetails(int id) => $"category:details:{id}";
    }
    private static class CacheTtl
    {
        public static readonly TimeSpan CategoryDetails = TimeSpan.FromMinutes(5);
    }
}