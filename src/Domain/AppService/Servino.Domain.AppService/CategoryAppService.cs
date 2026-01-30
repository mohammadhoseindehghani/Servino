using Microsoft.Extensions.Logging;
using Servino.Domain.Core._common;
using Servino.Domain.Core.CategoryAgg.Contracts.AppService;
using Servino.Domain.Core.CategoryAgg.Contracts.Service;
using Servino.Domain.Core.CategoryAgg.Dtos;

namespace Servino.Domain.AppService;

public class CategoryAppService(ICategoryService categoryService, ILogger<CategoryAppService> logger) : ICategoryAppService
{
    public async Task<Result<bool>> CreateAsync(CategoryDto command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.Title))
            return Result<bool>.Failure("عنوان الزامی است.");
        
        if (command.Title.Length <3)
            return Result<bool>.Failure("عنوان نمیتواند کمتر از 3 کاراکتر باشد.");
        
        try
        {
            var isCreated = await categoryService.CreateAsync(command, ct);

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
            var category = await categoryService.GetByIdAsync(id, ct);

            return category is null ? Result<CategoryDto>.Failure("دسته‌بندی مورد نظر یافت نشد.", "404") 
                : Result<CategoryDto>.Success(category);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "System error in CategoryAppService.GetByIdAsync | CategoryId: {CategoryId}",
                id);
            return Result<CategoryDto>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }

    public async Task<List<CategorySummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        return await categoryService.GetAllAsync(search, ct);
    }

    public async Task<int> GetCountAsync(CancellationToken ct)
    {
        return await categoryService.GetCountAsync(ct);
    }

    public async Task<List<CategoryClientDto>> GetCategoriesByParentIdAsync(int? parentId, CancellationToken ct)
    {
        return await categoryService.GetCategoriesByParentIdAsync(parentId, ct);
    }

    public async Task<List<ServiceClientDto>> GetServicesByCategoryIdAsync(int categoryId, CancellationToken ct)
    {
        var exists = await categoryService.IsCategoryExistAndActiveAsync(categoryId, ct);
        if (!exists)
        {
            return [];
        }

        return await categoryService.GetServicesByCategoryIdAsync(categoryId, ct);
    }

    public async Task<List<BreadcrumbDto>> GetBreadcrumbAsync(int categoryId, CancellationToken ct)
    {
        return await categoryService.GetBreadcrumbAsync(categoryId, ct);
    }
}