using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Domain.Core.UserAgg.Contracts.Service;

public interface ICustomerService
{
    Task<bool> CreateAsync(int userId, CancellationToken ct);
    Task<int> GetCustomerIdByUserIdAsync(int userId, CancellationToken ct);
    Task<CustomerProfileDto?> GetByUserIdAsync(int userId, CancellationToken ct);
    Task<bool> UpdateProfile(UpdateCustomerProfileDto command, CancellationToken ct);
}