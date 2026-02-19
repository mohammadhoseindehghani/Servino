using Microsoft.Extensions.Logging;
using Servino.Domain.Core._common;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Domain.AppService.UserAgg;

public class CustomerAppService(ICustomerService customerService, ILogger<CustomerAppService> logger) : ICustomerAppService
{
    public async Task<Result<CustomerProfileDto>> GetByUserIdAsync(
        int userId, CancellationToken ct)
    {
        try
        {
            var profile = await customerService.GetByUserIdAsync(userId, ct);

            return profile == null
                ? Result<CustomerProfileDto>.Failure("اطلاعات یافت نشد.")
                : Result<CustomerProfileDto>.Success(profile);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "System error in CustomerAppService.GetByUserIdAsync | UserId: {UserId}", userId);

            return Result<CustomerProfileDto>
                .Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }

    public async Task<Result<bool>> UpdateProfile(
        UpdateCustomerProfileDto command, CancellationToken ct)
    {
        var result = await customerService.UpdateProfile(command, ct);

        return !result
            ? Result<bool>.Failure("آپدیت انجام نشد")
            : Result<bool>.Success(true, "آپدیت با موفقیت انجام شد.");
    }
}