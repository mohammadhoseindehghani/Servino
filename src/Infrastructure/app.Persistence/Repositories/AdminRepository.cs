using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.DTOs.UserDTOs;
using Microsoft.EntityFrameworkCore;

namespace app.Persistence.Repositories;

public class AdminRepository(AppDbContext context) : IAdminRepository
{
    public async Task<AdminProfileDto?> GetByUserIdAsync(int userId, CancellationToken ct)
    {
        return await context.Admins.Where(c => c.UserId == userId)
            .Select(c => new AdminProfileDto()
                {
                    UserId = c.UserId,
                    AdminId = c.Id,
                    CityName = c.User.City!.Title,
                    FirstName = c.User.FirstName,
                    LastName = c.User.LastName,
                    Balance = c.User.Balance,
                    Email = c.User.Email,
                    Phone = c.User.MobileNumber,
                    RegisterDate = c.CreatedAt,
                    ProfileImagePath = c.User.ProfileImagePath
                }
            ).FirstOrDefaultAsync(ct);
    }
}