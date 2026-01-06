using Servino.Domain.Core.UserAgg.Contracts.Data;
using Servino.Domain.Core.UserAgg.Contracts.Service;

namespace Servino.Domain.Service;

public class CustomerService(ICustomerRepository customerRepo) : ICustomerService
{
    public async Task<int> GetCustomerIdByUserIdAsync(int userId, CancellationToken ct)
    {
        return await customerRepo.GetCustomerIdByUserIdAsync(userId, ct);
    }
}