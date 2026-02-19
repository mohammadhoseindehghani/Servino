using Servino.Domain.Core._common;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Domain.Core.UserAgg.Contracts.AppService;

public interface IAdminAppService
{
    Task<Result<AdminProfileDto>> GetByUserIdAsync(int userId, CancellationToken ct);
}