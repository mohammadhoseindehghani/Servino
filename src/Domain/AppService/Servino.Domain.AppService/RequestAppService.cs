using Microsoft.Extensions.Logging;
using Servino.Domain.Core._common;
using Servino.Domain.Core.ExpertHomeServiceAgg.Contracts.Service;
using Servino.Domain.Core.RequestAgg.Contracts.AppService;
using Servino.Domain.Core.RequestAgg.Contracts.Service;
using Servino.Domain.Core.RequestAgg.Dtos;
using Servino.Domain.Core.RequestAgg.Enum;
using Servino.Domain.Core.SuggestionAgg.Contracts.Service;
using Servino.Domain.Core.SuggestionAgg.Dtos;
using Servino.Domain.Core.SuggestionAgg.Enum;
using Servino.Domain.Core.UserAgg.Contracts.Service;

namespace Servino.Domain.AppService;

public class RequestAppService(
        IRequestService requestService,
        IExpertService expertService,
        IExpertHomeServiceService expertHomeServiceService,
        ISuggestionService suggestionService,
        IUserService userService,
        ILogger<RequestAppService> logger) : IRequestAppService
{
    public async Task<Result<int>> CreateAsync(CreateRequestDto command, CancellationToken ct)
    {
        try
        {
            if (command.DateRequired.Date < DateTime.Now.Date)
                return Result<int>.Failure("تاریخ درخواست نمی‌تواند در گذشته باشد.");

            if (command.ImagePaths != null && command.ImagePaths.Count > 5)
                return Result<int>.Failure("حداکثر ۵ تصویر می‌توانید آپلود کنید.");

            if (command.CityId <= 0)
                return Result<int>.Failure("انتخاب شهر الزامی است.");

            if (string.IsNullOrWhiteSpace(command.Address))
                return Result<int>.Failure("ادرس نمیتواند خالی باشد.");

            if (command.CustomerId <= 0)
                return Result<int>.Failure("برای این درخواست مشتری معتبر نیست.");

            if (command.HomeServiceId <= 0)
                return Result<int>.Failure("انتخاب خدمات الزامی است.");

            var newId = await requestService.CreateAsync(command, ct);

            return Result<int>.Success(newId, "درخواست با موفقیت ثبت شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "System error in RequestAppService.CreateAsync | CustomerId: {CustomerId} | HomeServiceId: {HomeServiceId}",
                command.CustomerId, command.HomeServiceId);

            return Result<int>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }

    public async Task<Result<bool>> UpdateAsync(UpdateRequestDto command, CancellationToken ct)
    {
        try
        {
            var existingRequest = await requestService.GetByIdAsync(command.Id, ct);
            if (existingRequest == null)
                return Result<bool>.Failure("درخواست مورد نظر یافت نشد.", "404");

            if (existingRequest.Status == RequestStatus.Done || existingRequest.Status == RequestStatus.Paid || existingRequest.Status == RequestStatus.Canceled)
            {
                return Result<bool>.Failure("امکان ویرایش درخواست‌های تکمیل یا لغو شده وجود ندارد.");
            }

            var isUpdated = await requestService.UpdateAsync(command, ct);

            return !isUpdated ? Result<bool>.Failure("عملیات ویرایش انجام نشد.")
                : Result<bool>.Success(true, "درخواست با موفقیت ویرایش شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "System error in RequestAppService.UpdateAsync | RequestId: {RequestId}",
                command.Id);

            return Result<bool>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }

    }

    public async Task<Result<RequestFullDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        try
        {
            var request = await requestService.GetByIdAsync(id, ct);

            return request == null
                ? Result<RequestFullDto>.Failure("درخواست یافت نشد.", "404")
                : Result<RequestFullDto>.Success(request);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "System error in RequestAppService.GetByIdAsync | RequestId: {RequestId}", id);
            return Result<RequestFullDto>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }

    public async Task<Result<RequestDetailDto>> GetDetailsByIdAsync(int id, CancellationToken ct)
    {
        try
        {
            var details = await requestService.GetDetailsByIdAsync(id, ct);

            return details == null
                ? Result<RequestDetailDto>.Failure("جزئیات درخواست یافت نشد.", "404")
                : Result<RequestDetailDto>.Success(details);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "System error in RequestAppService.GetDetailsByIdAsync | RequestId: {RequestId}", id);
            return Result<RequestDetailDto>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }

    public async Task<List<RequestSummaryDto>> GetAllAsync(PaginationRequestDto search, int? categoryId, int? cityId, CancellationToken ct)
    {
        return await requestService.GetAllAsync(search, categoryId, cityId, ct);
    }

    public async Task<List<RequestSummaryDto>> GetByCustomerIdAsync(int customerId, CancellationToken ct)
    {
        return await requestService.GetByCustomerIdAsync(customerId, ct);
    }

    public async Task<List<RequestSummaryDto>> GetAvailableForExpertAsync(int expertId, CancellationToken ct)
    {
        var expertProfile = await expertService.GetByUserId(expertId, ct);
        if (expertProfile == null || expertProfile.CityId == null) return [];

        var skillIds = await expertHomeServiceService.GetSelectedServiceIdsAsync(expertProfile.ExpertId, ct);
        if (skillIds == null || !skillIds.Any()) return [];

        return await requestService.GetAvailableForExpertAsync(expertProfile.ExpertId, skillIds, expertProfile.CityId.Value, ct);
    }

    public async Task<List<RequestSummaryDto>> GetAvailableForExpertAsync(int expertId, List<int> expertServiceIds, int cityId, CancellationToken ct)
    {
        return await requestService.GetAvailableForExpertAsync(expertId, expertServiceIds, cityId, ct);
    }

    public async Task<Result<bool>> CancelRequestAsync(int requestId, int customerId, CancellationToken ct)
    {
        try
        {
            var request = await requestService.GetByIdAsync(requestId, ct);
            if (request == null)
                return Result<bool>.Failure("درخواست یافت نشد.");

            if (request.CustomerId != customerId)
                return Result<bool>.Failure("شما دسترسی به لغو این درخواست را ندارید.");

            if (request.Status != RequestStatus.WaitingForExperts && request.Status != RequestStatus.WaitingForSelection)
            {
                return Result<bool>.Failure("تنها درخواست‌های در حال انتظار قابل لغو هستند. برای مراحل جلوتر با پشتیبانی تماس بگیرید.");
            }

            var updateDto = new UpdateRequestDto
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                Address = request.Address,
                CityId = request.CityId,
                DateRequired = request.DateRequired,
                Status = RequestStatus.Canceled,
                WinnerSuggestionId = request.WinnerSuggestionId
            };

            var result = await requestService.UpdateAsync(updateDto, ct);

            return result
                ? Result<bool>.Success(true, "درخواست شما لغو شد.")
                : Result<bool>.Failure("خطا در لغو درخواست.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "System error in RequestAppService.CancelRequestAsync | RequestId: {RequestId} | CustomerId: {CustomerId}",
                requestId, customerId);

            return Result<bool>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }

    }

    public async Task<Result<bool>> MarkAsDoneAndPayAsync(int requestId, int customerId, CancellationToken ct)
    {
        try
        {
            var request = await requestService.GetByIdAsync(requestId, ct);
            if (request == null) return Result<bool>.Failure("درخواست یافت نشد.");

            if (request.CustomerUserId != customerId) return Result<bool>.Failure("دسترسی غیرمجاز.");

            if (request.Status == RequestStatus.Paid || request.Status == RequestStatus.Done)
                return Result<bool>.Failure("این سفارش قبلاً پرداخت شده است.");

            if (request.Status != RequestStatus.Started)
                return Result<bool>.Failure("وضعیت سفارش برای پرداخت معتبر نیست. (باید در حالت شروع شده باشد)");

            if (request.WinnerSuggestionId == null)
                return Result<bool>.Failure("پیشنهاد تایید شده‌ای وجود ندارد.");

            var suggestion = await suggestionService.GetByIdAsync(request.WinnerSuggestionId.Value, ct);
            if (suggestion == null) return Result<bool>.Failure("اطلاعات پیشنهاد یافت نشد.");

            decimal totalAmount = suggestion.SuggestedPrice;

            var customerUser = await userService.GetByIdAsync(request.CustomerUserId, ct);
            if (customerUser.BalanceAmount < totalAmount)
            {
                return Result<bool>.Failure($"موجودی ناکافی است. مبلغ: {totalAmount:N0}، موجودی شما: {customerUser.BalanceAmount:N0}");
            }

            decimal adminShare = totalAmount * 0.10m;
            decimal expertShare = totalAmount * 0.90m;
            int adminUserId = 1;

            await userService.ChangeBalanceAsync(request.CustomerUserId, -totalAmount, ct);
            await userService.ChangeBalanceAsync(suggestion.ExpertUserId, expertShare, ct);
            await userService.ChangeBalanceAsync(adminUserId, adminShare, ct);

            var updateDto = new UpdateRequestDto
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                Address = request.Address,
                CityId = request.CityId,
                DateRequired = request.DateRequired,
                WinnerSuggestionId = request.WinnerSuggestionId,

                Status = RequestStatus.Paid,
                DateDone = DateTime.Now
            };

            await requestService.UpdateAsync(updateDto, ct);

            return Result<bool>.Success(true, "پرداخت با موفقیت انجام شد و سفارش بسته شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in MarkAsDoneAndPayAsync | RequestId: {RequestId}", requestId);
            return Result<bool>.Failure($"خطای سیستمی: {ex.Message}");
        }
    }

    public async Task<Result<bool>> SelectExpertAsync(int requestId, int suggestionId, int customerId, CancellationToken ct)
    {
        try
        {
            var request = await requestService.GetByIdAsync(requestId, ct);
            if (request == null) return Result<bool>.Failure("درخواست یافت نشد.");

            if (request.CustomerUserId != customerId) return Result<bool>.Failure("شما اجازه دسترسی به این درخواست را ندارید.");

            if (request.Status != RequestStatus.WaitingForExperts && request.Status != RequestStatus.WaitingForSelection)
                return Result<bool>.Failure("وضعیت درخواست برای انتخاب متخصص معتبر نیست.");

            var suggestion = await suggestionService.GetByIdAsync(suggestionId, ct);
            if (suggestion == null) return Result<bool>.Failure("پیشنهاد یافت نشد.");

            if (suggestion.RequestId != requestId) return Result<bool>.Failure("این پیشنهاد مربوط به این درخواست نیست.");

            var userDetail = await userService.GetByIdAsync(request.CustomerUserId, ct);

            if (userDetail == null) return Result<bool>.Failure("اطلاعات کاربر یافت نشد.");

            if (userDetail.BalanceAmount < suggestion.SuggestedPrice)
            {
                return Result<bool>.Failure($"موجودی ناکافی است. هزینه: {suggestion.SuggestedPrice:N0}، موجودی شما: {userDetail.BalanceAmount:N0}");
            }

            var updateSuggestionDto = new UpdateSuggestionDto
            {
                Id = suggestion.Id,
                Status = SuggestionStatus.Accepted,

                SuggestedPrice = suggestion.SuggestedPrice,
                SuggestedDate = suggestion.SuggestedDate,
                EstimatedDurationHours = suggestion.EstimatedDurationHours,
                Note = suggestion.Note
            };

            await suggestionService.UpdateAsync(updateSuggestionDto, ct);

            var updateRequestDto = new UpdateRequestDto
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                Address = request.Address,
                CityId = request.CityId,
                DateRequired = request.DateRequired,
                Status = RequestStatus.Started,
                WinnerSuggestionId = suggestionId
            };

            await requestService.UpdateAsync(updateRequestDto, ct);

            return Result<bool>.Success(true, "متخصص با موفقیت انتخاب شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in SelectExpertAsync | RequestId: {RequestId}", requestId);
            return Result<bool>.Failure("خطای سیستمی.");
        }
    }
}