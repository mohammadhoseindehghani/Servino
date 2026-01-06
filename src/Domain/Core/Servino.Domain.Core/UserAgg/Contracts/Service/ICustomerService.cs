namespace Servino.Domain.Core.UserAgg.Contracts.Service;

public interface ICustomerService
{
    Task<int> GetCustomerIdByUserIdAsync(int userId, CancellationToken ct);
}