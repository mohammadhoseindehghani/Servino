using Microsoft.Extensions.Logging;
using Servino.Domain.Core._common;
using Servino.Domain.Core.HomeServiceAgg.Contracts.AppService;
using Servino.Domain.Core.HomeServiceAgg.Contracts.Service;
using Servino.Domain.Core.HomeServiceAgg.Dtos;
using Servino.Framework.Caching;

namespace Servino.Domain.AppService;

public class HomeServiceAppService(IHomeServiceService homeServiceService,
    ILogger<HomeServiceAppService> logger, ICacheService cache) : IHomeServiceAppService
{
    public async Task<Result<bool>> CreateAsync(HomeServiceDto command, CancellationToken ct)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(command.Title))
                return Result<bool>.Failure("عنوان خدمت نمی‌تواند خالی باشد.");

            if (command.BasePrice < 0)
                return Result<bool>.Failure("قیمت پایه نمی‌تواند منفی باشد.");

            if (command.CategoryId <= 0)
                return Result<bool>.Failure("انتخاب دسته‌بندی الزامی است.");

            var isCreated = await homeServiceService.CreateAsync(command, ct);

            await cache.RemoveAsync(CacheKeys.HomeServicesAll(command.Title, 1, 10), ct);

            return !isCreated ? Result<bool>.Failure("خطایی در ثبت خدمت رخ داد.") 
                : Result<bool>.Success(true, "خدمت جدید با موفقیت ثبت شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "System error occurred while creating HomeService. CategoryId: {CategoryId}, Title: {Title}",
                command?.CategoryId,
                command?.Title);

            return Result<bool>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }

    public async Task<Result<bool>> UpdateAsync(HomeServiceDto command, CancellationToken ct)
    {
        try
        {
            if (command.Id <= 0)
                return Result<bool>.Failure("شناسه خدمت نامعتبر است.");

            if (command.BasePrice < 0)
                return Result<bool>.Failure("قیمت پایه نمی‌تواند منفی باشد.");

            var isUpdated = await homeServiceService.UpdateAsync(command, ct);

            if (isUpdated)
            {
                await cache.RemoveAsync(CacheKeys.HomeServiceDetails(command.Id), ct);
                await cache.RemoveAsync(CacheKeys.HomeServicesAll(command.Title, 1, 10), ct);
            }

            return !isUpdated ? Result<bool>.Failure("خدمت یافت نشد یا ویرایش انجام نشد.") 
                : Result<bool>.Success(true, "خدمت با موفقیت ویرایش شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "System error occurred while updating HomeService. ServiceId: {ServiceId}",
                command?.Id);

            return Result<bool>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }

    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken ct)
    {
        try
        {

            var isDeleted = await homeServiceService.DeleteAsync(id, ct);


            if (isDeleted)
            {
                await cache.RemoveAsync(CacheKeys.HomeServiceDetails(id), ct);
                await cache.RemoveAsync(CacheKeys.HomeServicesAll("", 1, 10), ct);
            }

            return !isDeleted ? Result<bool>.Failure("خدمت یافت نشد.") 
                : Result<bool>.Success(true, "خدمت با موفقیت حذف شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "System error occurred while deleting HomeService. ServiceId: {ServiceId}",
                id);

            return Result<bool>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }

    public async Task<Result<HomeServiceDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        try
        {
            var key = CacheKeys.HomeServiceDetails(id);
            var service = await cache.GetOrSetAsync(key,
                async () => await homeServiceService.GetByIdAsync(id, ct), CacheTtl.HomeServiceDetails, ct);

            return service == null
                ? Result<HomeServiceDto>.Failure("خدمت مورد نظر یافت نشد.", "404")
                : Result<HomeServiceDto>.Success(service);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "System error occurred while retrieving HomeService by id. ServiceId: {ServiceId}", id);
            return Result<HomeServiceDto>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }


    public async Task<Result<List<HomeServiceSummaryDto>>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        var key = CacheKeys.HomeServicesAll(search.SearchKey ?? "", search.PageNumber, search.PageSize);

        return await cache.GetOrSetAsync(
            key,
            async () => await homeServiceService.GetAllAsync(search, ct),
            CacheTtl.HomeServices,
            ct);
    }






    private static class CacheKeys
    {
        public static string HomeServiceDetails(int id) => $"homeService:details:{id}";
        public static string HomeServicesAll(string searchKey, int pageNumber, int pageSize) =>
            $"homeServices:all:{searchKey}:{pageNumber}:{pageSize}";
    }

    private static class CacheTtl
    {
        public static readonly TimeSpan HomeServiceDetails = TimeSpan.FromMinutes(5);
        public static readonly TimeSpan HomeServices = TimeSpan.FromMinutes(10);
    }

}