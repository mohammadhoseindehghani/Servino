using Servino.Domain.Core._common;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Domain.Core.UserAgg.Contracts.AppService;

public interface IExpertAppService
{
    Task<Result<ExpertProfileDto>> GetByUserId(int userId, CancellationToken ct);
    Task<Result<ExpertProfileDto>> GetByExpertId(int expertId, CancellationToken ct);
    Task<Result<bool>> UpdateProfile(UpdateExpertProfileDto command, CancellationToken ct);
    Task<Result<List<ExpertServiceItemDto>>> GetServicesForEditAsync(int userId, CancellationToken ct);
    Task<Result<bool>> UpdateServicesAsync(int userId, List<int> selectedIds, CancellationToken ct);
}