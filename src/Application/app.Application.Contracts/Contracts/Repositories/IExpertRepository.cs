using app.Application.Contracts.DTOs.UserDTOs;

namespace app.Application.Contracts.Contracts.Repositories;

public interface IExpertRepository
{
    Task<bool> CreateAsync(int userId, CancellationToken ct); 
    Task<int> GetIdByUserIdAsync(int userId, CancellationToken ct);
    Task<ExpertProfileDto?> GetByUserIdAsync(int userId, CancellationToken ct);
    Task<ExpertProfileDto?> GetByExpertIdAsync(int expertId, CancellationToken ct);
    Task<bool> UpdateProfile(UpdateExpertProfileDto command, CancellationToken ct);
    Task<bool> HardDeleteByUserIdAsync(int userId, CancellationToken ct);
}