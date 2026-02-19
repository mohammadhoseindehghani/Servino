using Microsoft.Extensions.Logging;
using Servino.Domain.Core._common;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Dtos;
using Servino.Framework.Caching;

namespace Servino.Domain.AppService.UserAgg;

public class AdminAppService(IAdminService adminService, ILogger<AdminAppService> logger) : IAdminAppService
{
    public async Task<Result<AdminProfileDto>> GetByUserIdAsync(
        int userId, CancellationToken ct)
    {
        try
        {
            var profile = await adminService.GetByUserIdAsync(userId, ct);

            return profile == null
                ? Result<AdminProfileDto>.Failure("اطلاعات یافت نشد.")
                : Result<AdminProfileDto>.Success(profile);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "System error in AdminAppService.GetByUserIdAsync | UserId: {UserId}", userId);

            return Result<AdminProfileDto>
                .Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }
}