using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Providers_Services;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.CategoryAgg;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Categories.Commands.Delete;

public class DeleteCategoryCommandHandler(
    ICategoryService categoryService,
    ICacheService cache,
    ILogger<DeleteCategoryCommandHandler> logger,
    IFileService fileService) : IRequestHandler<DeleteCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken ct)
    {
        try
        {

            var category = await categoryService.GetByIdAsync(request.Id, ct);

            if (category == null)
                return Result.Failure("دسته بندی یافت نشد.", "NOT_FOUND");

            if (category.ImagePath != null)
                await fileService.DeleteFileAsync(category.ImagePath, ct);

            await categoryService.DeleteAsync(request.Id, ct);

            await cache.RemoveAsync(CacheKeys.CategoryDetails(request.Id), ct);
            await cache.BumpStampAsync(CacheKeys.StampAllRequests, CacheTtl.Stamps, ct);
            await cache.BumpStampAsync(CacheKeys.StampAvailableRequests, CacheTtl.Stamps, ct);

            return Result.Success("دسته بندی با موفقیت حذف شد.");

        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "System error in CategoryAppService.DeleteAsync | CategoryId: {CategoryId}",
                request.Id);
            return Result.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }

    private static class CacheKeys
    {
        public static string CategoryDetails(int id) => $"category:details:{id}";
        public static string StampAllRequests => "stamp:requests:all";
        public static string StampAvailableRequests => "stamp:requests:available";
    }
    private static class CacheTtl
    {
        public static readonly TimeSpan Stamps = TimeSpan.FromHours(6);
    }
}