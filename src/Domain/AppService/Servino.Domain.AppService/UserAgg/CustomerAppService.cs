using Servino.Domain.Core._common;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Domain.AppService.UserAgg;

public class CustomerAppService(ICustomerService customerService) : ICustomerAppService
{
    public async Task<Result<CustomerProfileDto>> GetByUserIdAsync(int userId, CancellationToken ct)
    {
        var result = await customerService.GetByUserIdAsync(userId, ct);
        return result is null ? Result<CustomerProfileDto>.Failure("اطلاعات یافت نشد.") : Result<CustomerProfileDto>.Success(result);
    }

    public async Task<Result<bool>> UpdateProfile(UpdateCustomerProfileDto command, CancellationToken ct)
    {
        var result = await customerService.UpdateProfile(command, ct);
        return !result ? Result<bool>.Failure("اپدیت انجام نشد") : Result<bool>.Success(result,"اپدیت با موفقیت انحام شد.");
    }
}