using Microsoft.Extensions.Logging;
using Servino.Domain.Core._common;
using Servino.Domain.Core.CategoryAgg.Contracts.AppService;
using Servino.Domain.Core.CategoryAgg.Contracts.Service;
using Servino.Domain.Core.CategoryAgg.Dtos;
using Servino.Framework.Caching;

namespace Servino.Domain.AppService;

public class CategoryAppService(ICategoryService categoryService, ILogger<CategoryAppService> logger,
    ICacheService cache) : ICategoryAppService
{
    public async Task<Result<bool>> CreateAsync(CategoryDto command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.Title))
            return Result<bool>.Failure("عنوان الزامی است.");

        if (command.Title.Length < 3)
            return Result<bool>.Failure("عنوان نمیتواند کمتر از 3 کاراکتر باشد.");

        try
        {
            var isCreated = await categoryService.CreateAsync(command, ct);

            await cache.BumpStampAsync(CacheKeys.StampAllRequests, CacheTtl.Stamps, ct);
            await cache.BumpStampAsync(CacheKeys.StampAvailableRequests, CacheTtl.Stamps, ct);


            return !isCreated ? Result<bool>.Failure("خطایی در ایجاد دسته‌بندی رخ داد. ممکن است عنوان تکراری باشد.")
                : Result<bool>.Success(true, "دسته‌بندی با موفقیت ایجاد شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "System error in CategoryAppService.CreateAsync | Title: {Title} | ParentId: {ParentId}",
                command.Title, command.ParentId);
            return Result<bool>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }

    public async Task<Result<bool>> UpdateAsync(CategoryDto command, CancellationToken ct)
    {
        try
        {
            var isUpdated = await categoryService.UpdateAsync(command, ct);

            if (isUpdated)
            {
                await cache.RemoveAsync(CacheKeys.CategoryDetails(command.Id), ct);
                await cache.BumpStampAsync(CacheKeys.StampAllRequests, CacheTtl.Stamps, ct);
                await cache.BumpStampAsync(CacheKeys.StampAvailableRequests, CacheTtl.Stamps, ct);
            }

            return !isUpdated ? Result<bool>.Failure("دسته‌بندی یافت نشد یا ویرایش انجام نشد.")
                : Result<bool>.Success(true, "دسته‌بندی با موفقیت ویرایش شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "System error in CategoryAppService.UpdateAsync | CategoryId: {CategoryId} | Title: {Title}",
                command.Id, command.Title);
            return Result<bool>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }

    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken ct)
    {
        try
        {
            var isDeleted = await categoryService.DeleteAsync(id, ct);

            if (isDeleted)
            {
                await cache.RemoveAsync(CacheKeys.CategoryDetails(id), ct);
                await cache.BumpStampAsync(CacheKeys.StampAllRequests, CacheTtl.Stamps, ct);
                await cache.BumpStampAsync(CacheKeys.StampAvailableRequests, CacheTtl.Stamps, ct);
            }

            return !isDeleted ? Result<bool>.Failure("دسته‌بندی یافت نشد یا قابل حذف نیست (ممکن است دارای زیرمجموعه باشد).")
                : Result<bool>.Success(true, "دسته‌بندی با موفقیت حذف شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "System error in CategoryAppService.DeleteAsync | CategoryId: {CategoryId}",
                id);
            return Result<bool>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }

    public async Task<Result<CategoryDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        try
        {
            var key = CacheKeys.CategoryDetails(id);
            var category = await cache.GetOrSetAsync(
                key, async () => await categoryService.GetByIdAsync(id, ct),
                CacheTtl.CategoryDetails, ct);

            return category == null
                ? Result<CategoryDto>.Failure("دسته‌بندی مورد نظر یافت نشد.", "404")
                : Result<CategoryDto>.Success(category);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "System error in CategoryAppService.GetByIdAsync | CategoryId: {CategoryId}", id);
            return Result<CategoryDto>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }


    public async Task<List<CategorySummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        var key = CacheKeys.CategoriesAll(search.SearchKey ?? "", search.PageNumber, search.PageSize);

        return await cache.GetOrSetAsync(
            key,
            async () => await categoryService.GetAllAsync(search, ct), CacheTtl.Categories, ct);
    }


    public async Task<int> GetCountAsync(CancellationToken ct)
    {
        return await categoryService.GetCountAsync(ct);
    }

    public async Task<List<CategoryClientDto>> GetCategoriesByParentIdAsync(int? parentId, CancellationToken ct)
    {
        var key = CacheKeys.CategoriesByParentId(parentId ?? 0);

        return await cache.GetOrSetAsync(key, async () => await categoryService.GetCategoriesByParentIdAsync(parentId, ct), 
            CacheTtl.Categories, ct);
    }


    public async Task<List<ServiceClientDto>> GetServicesByCategoryIdAsync(int categoryId, CancellationToken ct)
    {
        var key = CacheKeys.ServicesByCategoryId(categoryId);

        var exists = await categoryService.IsCategoryExistAndActiveAsync(categoryId, ct);
        if (!exists)
        {
            return [];
        }

        return await cache.GetOrSetAsync(
            key, async () => await categoryService.GetServicesByCategoryIdAsync(categoryId, ct),
            CacheTtl.Services, ct);
    }


    public async Task<List<BreadcrumbDto>> GetBreadcrumbAsync(int categoryId, CancellationToken ct)
    {
        var key = CacheKeys.Breadcrumb(categoryId);

        return await cache.GetOrSetAsync(
            key, async () => await categoryService.GetBreadcrumbAsync(categoryId, ct),
            CacheTtl.Breadcrumb, ct);
    }





    private static class CacheKeys
    {
        public static string CategoryDetails(int id) => $"category:details:{id}";
        public static string CategoriesAll(string searchKey, int pageNumber, int pageSize) =>
            $"categories:all:{searchKey}:{pageNumber}:{pageSize}";
        public static string CategoriesByParentId(int parentId) => $"categories:parent:{parentId}";
        public static string ServicesByCategoryId(int categoryId) => $"services:category:{categoryId}";
        public static string Breadcrumb(int categoryId) => $"breadcrumb:{categoryId}";

        public static string StampAllRequests => "stamp:requests:all"; 
        public static string StampAvailableRequests => "stamp:requests:available"; 
    }
    private static class CacheTtl
    {
        public static readonly TimeSpan CategoryDetails = TimeSpan.FromMinutes(5);
        public static readonly TimeSpan Categories = TimeSpan.FromMinutes(10);
        public static readonly TimeSpan Services = TimeSpan.FromMinutes(10);
        public static readonly TimeSpan Breadcrumb = TimeSpan.FromMinutes(5);
        public static readonly TimeSpan Stamps = TimeSpan.FromHours(6);  

    }
}