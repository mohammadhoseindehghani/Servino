using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.DTOs.UserDTOs;
using app.Domain.UserAgg.Entities;
using Microsoft.EntityFrameworkCore;

namespace app.Persistence.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<bool> IsMobileExistAsync(string mobile, CancellationToken ct)
    {
        return await context.Users.AnyAsync(u => u.MobileNumber == mobile, ct);
    }

    public async Task<bool> UpdateProfileAsync(UpdateUserDto command, CancellationToken ct)
    {
        var affectedRows = await context.Users
            .Where(u => u.Id == command.Id && !u.IsDeleted)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.FirstName, command.FirstName)
                    .SetProperty(u => u.LastName, command.LastName)
                    .SetProperty(u => u.CityId, command.CityId)
                    .SetProperty(u => u.MobileNumber, command.Mobile)
                    .SetProperty(u => u.ProfileImagePath,
                        u => command.ProfileImagePath ?? u.ProfileImagePath)
                    .SetProperty(u => u.UpdatedAt, DateTime.UtcNow),
                ct);
        return affectedRows > 0;
    }

    public async Task<bool> CreateAsync(CreateUserDto command, CancellationToken ct)
    {
        var user = new User
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            MobileNumber = command.Mobile,
            IdentityId = command.IdentityId,
            CityId = command.CityId,
            Balance = 0,
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        context.Users.Add(user);
        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(UpdateUserDto command, CancellationToken ct)
    {
        var affectedRows = await context.Users
            .Where(u => u.Id == command.Id)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.FirstName, command.FirstName)
                    .SetProperty(u => u.LastName, command.LastName)
                    .SetProperty(u => u.MobileNumber, command.Mobile)
                    .SetProperty(u => u.CityId, command.CityId)
                    .SetProperty(u => u.ProfileImagePath,
                        u => command.ProfileImagePath ?? u.ProfileImagePath)
                    .SetProperty(u => u.UpdatedAt, DateTime.UtcNow),
                ct);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var affectedRows = await context.Users
            .Where(u => u.Id == id && !u.IsDeleted)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.IsDeleted, true)
                    .SetProperty(u => u.DeletedAt, DateTime.UtcNow)
                    .SetProperty(u => u.IsActive, false)
                    .SetProperty(u => u.Email, u => "deleted_" + u.Id + "_" + u.Email)
                    .SetProperty(u => u.MobileNumber, u => "deleted_" + u.Id + "_" + u.MobileNumber),
                ct);
        return affectedRows > 0;
    }

    public async Task<bool> HardDeleteAsync(int id, CancellationToken ct)
    {
        var affectedRows = await context.Users
            .Where(u => u.Id == id && !u.IsDeleted)
            .ExecuteDeleteAsync(ct);
        return affectedRows > 0;
    }

    public async Task<UserDetailDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new UserDetailDto
            {
                Id = u.Id,
                IdentityId = u.IdentityId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Mobile = u.MobileNumber,
                CityId = u.CityId,
                ProfileImagePath = u.ProfileImagePath,
                BalanceAmount = u.Balance,
                RegisterDate = u.CreatedAt
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<int> GetIdByIdentityIdAsync(string identityId, CancellationToken ct)
    {
        var id = await context.Users
            .Where(u => u.IdentityId == identityId)
            .Select(u => u.Id)
            .FirstOrDefaultAsync(ct);
        return id;  
    }

    public async Task<List<UserSummaryProjectionDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        var query = context.Users
            .AsNoTracking()
            .Where(u => !u.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search.SearchKey))
        {
            query = query.Where(u =>
                u.LastName.Contains(search.SearchKey) ||
                u.Email.Contains(search.SearchKey) ||
                u.MobileNumber.Contains(search.SearchKey));
        }

        return await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((search.PageNumber - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(u => new UserSummaryProjectionDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                MobileNumber = u.MobileNumber,
                CityTitle = u.City != null ? u.City.Title : null,
                Balance = u.Balance,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                ProfileImagePath = u.ProfileImagePath,
                CityId = u.CityId,
                ProvinceId = u.City!.ProvinceId,
                HasAdmin = u.Admin != null,
                HasExpert = u.Expert != null,
                HasCustomer = u.Customer != null
            })
            .ToListAsync(ct);
    }

    public async Task<bool> ChangeBalanceAsync(int userId, decimal amount, CancellationToken ct)
    {
        var affectedRows = await context.Users
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(u => u.Balance, u => u.Balance + amount)
                .SetProperty(u => u.UpdatedAt, DateTime.Now),
                ct);

        return affectedRows > 0;
    }

    public async Task<bool> IsEmailExistAsync(string email, CancellationToken ct)
    {
        return await context.Users.AnyAsync(u => u.Email == email, ct);
    }

    public async Task<int> GetCountAsync(CancellationToken ct)
    {
        return await context.Users.CountAsync(ct);
    }

    public async Task<string> GetUserProfileImageAsync(int userId, CancellationToken ct)
    {
        var profileImageUrl = await context.Users
            .Where(u => u.Id == userId)
            .Select(u => u.ProfileImagePath)
            .FirstOrDefaultAsync(ct);

        return profileImageUrl ?? string.Empty;
    }

    public async Task<bool> UpdateProfileImageAsync(int userId, string path, CancellationToken ct)
    {
        var effectedRows = await context.Users.Where(u => u.Id == userId)
            .ExecuteUpdateAsync(setter => setter
                .SetProperty(u => u.ProfileImagePath, path), ct);

        return effectedRows > 0;
    }
}
