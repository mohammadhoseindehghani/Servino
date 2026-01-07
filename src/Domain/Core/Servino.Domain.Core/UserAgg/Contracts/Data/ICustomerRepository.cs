namespace Servino.Domain.Core.UserAgg.Contracts.Data;

public interface ICustomerRepository
{
    Task<bool> CreateAsync(int userId, CancellationToken ct);
    Task<int> GetCustomerIdByUserIdAsync(int userId, CancellationToken ct);
}