namespace Servino.Domain.Core.UserAgg.Contracts.Data;

public interface ICustomerRepository
{
    Task<int> GetCustomerIdByUserIdAsync(int userId, CancellationToken ct);
}