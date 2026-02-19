using Servino.Domain.Core._common;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Domain.Core.UserAgg.Contracts.AppService;

public interface ICustomerAppService
{
    Task<Result<CustomerProfileDto>> GetByUserIdAsync(int userId, CancellationToken ct);
    Task<Result<bool>> UpdateProfile(UpdateCustomerProfileDto command, CancellationToken ct);
}