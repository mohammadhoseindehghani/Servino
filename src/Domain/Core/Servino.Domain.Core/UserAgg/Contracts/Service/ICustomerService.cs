namespace Servino.Domain.Core.UserAgg.Contracts.Service;

public interface ICustomerService
{
    Task<bool> CreateAsync(int userId, CancellationToken ct);
    Task<int> GetCustomerIdByUserIdAsync(int userId, CancellationToken ct);
}