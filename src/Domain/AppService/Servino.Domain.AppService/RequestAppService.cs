using Servino.Domain.Core._common;
using Servino.Domain.Core.ExpertHomeServiceAgg.Contracts.Service;
using Servino.Domain.Core.RequestAgg.Contracts.AppService;
using Servino.Domain.Core.RequestAgg.Contracts.Service;
using Servino.Domain.Core.RequestAgg.Dtos;
using Servino.Domain.Core.RequestAgg.Enum;
using Servino.Domain.Core.UserAgg.Contracts.Service;

namespace Servino.Domain.AppService;

public class RequestAppService(
        IRequestService requestService,
        IExpertService expertService, 
        IExpertHomeServiceService expertHomeServiceService) : IRequestAppService
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

            var newId = await requestService.CreateAsync(command, ct);

            return Result<int>.Success(newId, "درخواست با موفقیت ثبت شد.");
        }
        catch (Exception ex)
        {
            return Result<int>.Failure($"خطای سیستمی در ثبت درخواست: {ex.Message}");
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
            return Result<bool>.Failure($"خطای سیستمی: {ex.Message}");
        }
    }

    public async Task<Result<RequestFullDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        try
        {
            var request = await requestService.GetByIdAsync(id, ct);
            return request == null ? Result<RequestFullDto>.Failure("درخواست یافت نشد.", "404") 
                : Result<RequestFullDto>.Success(request);
        }
        catch (Exception ex)
        {
            return Result<RequestFullDto>.Failure($"خطا: {ex.Message}");
        }
    }

    public async Task<Result<RequestDetailDto>> GetDetailsByIdAsync(int id, CancellationToken ct)
    {
        try
        {
            var details = await requestService.GetDetailsByIdAsync(id, ct);
            return details == null ? Result<RequestDetailDto>.Failure("جزئیات درخواست یافت نشد.", "404") 
                : Result<RequestDetailDto>.Success(details);
        }
        catch (Exception ex)
        {
            return Result<RequestDetailDto>.Failure($"خطا: {ex.Message}");
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
        if (expertProfile == null || expertProfile.CityId == null)
        {
            return [];
        }

        var skillIds = await expertHomeServiceService.GetSelectedServiceIdsAsync(expertProfile.ExpertId, ct);
        if (skillIds == null! || !skillIds.Any())
        {
            return [];
        }

        return await requestService.GetAvailableForExpertAsync(skillIds, expertProfile.CityId.Value, ct);
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
            return Result<bool>.Failure($"خطا: {ex.Message}");
        }
    }
}