using Servino.Domain.Core._common;
using Servino.Domain.Core.CategoryAgg.Contracts.AppService;
using Servino.Domain.Core.CategoryAgg.Contracts.Service;
using Servino.Domain.Core.CategoryAgg.Dtos;

namespace Servino.Domain.AppService;

public class CategoryAppService(ICategoryService categoryService) : ICategoryAppService
{
    public async Task<Result<bool>> CreateAsync(CategoryDto command, CancellationToken ct)
    {
        //validation
        try
        {
            if (string.IsNullOrWhiteSpace(command.Title))
            {
                return Result<bool>.Failure("عنوان دسته‌بندی نمی‌تواند خالی باشد.");
            }

            var isCreated = await categoryService.CreateAsync(command, ct);

            if (!isCreated)
            {
                return Result<bool>.Failure("خطایی در ایجاد دسته‌بندی رخ داد. ممکن است عنوان تکراری باشد.");
            }

            return Result<bool>.Success(true, "دسته‌بندی با موفقیت ایجاد شد.");
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"خطای سیستمی: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UpdateAsync(CategoryDto command, CancellationToken ct)
    {
        try
        {
            var isUpdated = await categoryService.UpdateAsync(command, ct);

            if (!isUpdated)
            {
                return Result<bool>.Failure("دسته‌بندی یافت نشد یا ویرایش انجام نشد.");
            }

            return Result<bool>.Success(true, "دسته‌بندی با موفقیت ویرایش شد.");
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"خطای سیستمی: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken ct)
    {
        try
        {
            var isDeleted = await categoryService.DeleteAsync(id, ct);

            if (!isDeleted)
            {
                return Result<bool>.Failure("دسته‌بندی یافت نشد یا قابل حذف نیست (ممکن است دارای زیرمجموعه باشد).");
            }

            return Result<bool>.Success(true, "دسته‌بندی با موفقیت حذف شد.");
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"خطای سیستمی: {ex.Message}");
        }
    }

    public async Task<Result<CategoryDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        try
        {
            var category = await categoryService.GetByIdAsync(id, ct);

            if (category is null)
            {
                return Result<CategoryDto>.Failure("دسته‌بندی مورد نظر یافت نشد.", "404");
            }

            return Result<CategoryDto>.Success(category);
        }
        catch (Exception ex)
        {
            return Result<CategoryDto>.Failure($"خطای سیستمی: {ex.Message}");
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
}