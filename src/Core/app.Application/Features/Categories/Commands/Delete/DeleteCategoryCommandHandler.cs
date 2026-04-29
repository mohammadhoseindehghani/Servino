using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.Contracts.Services;
using app.Application.Features.Categories.Commands.Create;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Categories.Commands.Delete;

public class DeleteCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    ICacheService cache,
    ILogger<DeleteCategoryCommandHandler> logger) : IRequestHandler<DeleteCategoryCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteCategoryCommand request, CancellationToken ct)
    {
        try
        {
            var isDeleted = await categoryRepository.DeleteAsync(request.Id, ct);

            if (isDeleted)
            {
                await cache.RemoveAsync(CacheKeys.CategoryDetails(request.Id), ct);
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
                request.Id);
            return Result<bool>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
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