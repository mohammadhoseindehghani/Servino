using Servino.Domain.Core.UserAgg.Contracts.Data;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Domain.Service;

public class ExpertService(IExpertRepository expertRepo) : IExpertService
{
    public async Task<bool> CreateAsync(int userId, CancellationToken ct)
    {
       return await expertRepo.CreateAsync(userId, ct);
    }

    public async Task<ExpertProfileDto?> GetByUserId(int userId, CancellationToken ct)
    {
        return await expertRepo.GetByUserIdAsync(userId, ct);
    }

    public async Task<bool> UpdateProfile(UpdateExpertProfileDto command, CancellationToken ct)
    {
        return await expertRepo.UpdateProfile(command, ct);
    }

    public async Task<int> GetExpertIdByUserIdAsync(int userId, CancellationToken ct)
    {
        return await expertRepo.GetIdByUserIdAsync(userId, ct);
    }

    public async Task<bool> HardDeleteByUserIdAsync(int userId, CancellationToken ct)
    {
        return await expertRepo.HardDeleteByUserIdAsync(userId, ct);
    }
}