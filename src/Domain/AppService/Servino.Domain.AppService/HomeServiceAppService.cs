using Microsoft.Extensions.Logging;
using Servino.Domain.Core._common;
using Servino.Domain.Core.HomeServiceAgg.Contracts.AppService;
using Servino.Domain.Core.HomeServiceAgg.Contracts.Service;
using Servino.Domain.Core.HomeServiceAgg.Dtos;

namespace Servino.Domain.AppService;

public class HomeServiceAppService(IHomeServiceService homeServiceService, ILogger<HomeServiceAppService> logger) : IHomeServiceAppService
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
            var service = await homeServiceService.GetByIdAsync(id, ct);

            return service is null ? Result<HomeServiceDto>.Failure("خدمت مورد نظر یافت نشد.", "404") 
                : Result<HomeServiceDto>.Success(service);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "System error occurred while retrieving HomeService by id. ServiceId: {ServiceId}",
                id);

            return Result<HomeServiceDto>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }

    public async Task<Result<List<HomeServiceSummaryDto>>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        try
        {
            var services = await homeServiceService.GetAllAsync(search, ct);

            return Result<List<HomeServiceSummaryDto>>.Success(services);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "System error occurred while retrieving HomeService list. Page: {Page}, PageSize: {PageSize}",
                search.PageNumber,
                search.PageSize);

            return Result<List<HomeServiceSummaryDto>>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }
}