using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.UserAgg;
using app.Application.Contracts.DTOs.UserDTOs;

namespace app.Application.Features.Experts.Services;

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

    public async Task<ExpertProfileDto?> GetByExpertIdAsync(int expertId, CancellationToken ct)
    {
        return await expertRepo.GetByExpertIdAsync(expertId, ct);
    }

    public async Task<bool> HardDeleteByUserIdAsync(int userId, CancellationToken ct)
    {
        return await expertRepo.HardDeleteByUserIdAsync(userId, ct);
    }
}