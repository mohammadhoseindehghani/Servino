using Servino.Domain.Core.UserAgg.Contracts.Data;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Domain.Service;

public class CustomerService(ICustomerRepository customerRepo) : ICustomerService
{
    public async Task<bool> CreateAsync(int userId, CancellationToken ct)
    {
        return await customerRepo.CreateAsync(userId, ct);
    }

    public async Task<int> GetCustomerIdByUserIdAsync(int userId, CancellationToken ct)
    {
        return await customerRepo.GetCustomerIdByUserIdAsync(userId, ct);
    }

    public async Task<CustomerProfileDto?> GetByUserIdAsync(int userId, CancellationToken ct)
    {
        return await customerRepo.GetByUserIdAsync(userId, ct);
    }

    public async Task<bool> UpdateProfile(UpdateCustomerProfileDto command, CancellationToken ct)
    {
        return await customerRepo.UpdateProfile(command, ct);
    }

    public async Task<bool> HardDeleteByUserIdAsync(int userId, CancellationToken ct)
    {
        return await customerRepo.HardDeleteByUserIdAsync(userId, ct);
    }
}