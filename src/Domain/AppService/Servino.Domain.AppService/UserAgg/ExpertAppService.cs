using System.Linq.Expressions;
using Servino.Domain.Core._common;
using Servino.Domain.Core.ExpertHomeServiceAgg.Contracts.Service;
using Servino.Domain.Core.HomeServiceAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Domain.AppService.UserAgg;

public class ExpertAppService(
    IExpertService expertService,                   
    IHomeServiceService homeServiceService,        
    IExpertHomeServiceService expertHomeServiceService 
) : IExpertAppService
{
    public async Task<Result<ExpertProfileDto>> GetByUserId(int userId, CancellationToken ct)
    {
        var result = await expertService.GetByUserId(userId, ct);
        if (result is null)
        {
            return Result<ExpertProfileDto>.Failure("پروفایلی یافت نشد");
        }

        return Result<ExpertProfileDto>.Success(result);
    }

    public async Task<Result<bool>> UpdateProfile(UpdateExpertProfileDto command, CancellationToken ct)
    {
        var result = await expertService.UpdateProfile(command, ct);
        if (!result)
        {
            return Result<bool>.Failure("عملیات اپدیت با شکست مواجه شد");
        }

        return Result<bool>.Success(true, "اپدیت اطلاعات با موفقیت انجام شد");
    }

    public async Task<Result<List<ExpertServiceItemDto>>> GetServicesForEditAsync(int userId, CancellationToken ct)
    {
        var expertId = await expertService.GetExpertIdByUserIdAsync(userId, ct);
        if (expertId == 0) return Result<List<ExpertServiceItemDto>>.Failure("اکسپرت یافت نشد.");

        var allServices = await homeServiceService.GetAllActiveServicesAsync(ct);

        var myServiceIds = await expertHomeServiceService.GetSelectedServiceIdsAsync(expertId, ct);

        var result = allServices.Select(s => new ExpertServiceItemDto
        {
            HomeServiceId = s.Id,
            HomeServiceTitle = s.Title,
            IsSelected = myServiceIds.Contains(s.Id)
        }).ToList();

        return Result<List<ExpertServiceItemDto>>.Success(result);
    }

    public async Task<Result<bool>> UpdateServicesAsync(int userId, List<int> selectedIds, CancellationToken ct)
    {
        var expertId = await expertService.GetExpertIdByUserIdAsync(userId, ct);
        if (expertId == 0) return Result<bool>.Failure("اکسپرت یافت نشد.");

        try
        {
            await expertHomeServiceService.UpdateExpertServicesAsync(expertId, selectedIds, ct);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure("خطا: " + ex.Message);
        }
    }
}
