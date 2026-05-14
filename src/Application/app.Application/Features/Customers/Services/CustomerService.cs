using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.UserAgg;
using app.Application.Contracts.DTOs.UserDTOs;

namespace app.Application.Features.Customers.Services;

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