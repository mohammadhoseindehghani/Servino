using Servino.Domain.Core._common;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Domain.AppService.UserAgg;

public class AdminAppService(IAdminService adminService) : IAdminAppService
{
    public async Task<Result<AdminProfileDto>> GetByUserIdAsync(int userId, CancellationToken ct)
    {
        var result = await adminService.GetByUserIdAsync(userId, ct);
        return result is null ? Result<AdminProfileDto>.Failure("اطلاعات یافت نشد.") : Result<AdminProfileDto>.Success(result);
    }

}