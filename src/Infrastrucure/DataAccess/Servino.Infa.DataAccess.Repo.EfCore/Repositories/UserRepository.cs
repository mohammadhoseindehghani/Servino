using Microsoft.EntityFrameworkCore;
using Servino.Domain.Core._common;
using Servino.Domain.Core.UserAgg.Contracts.Data;
using Servino.Domain.Core.UserAgg.Dtos;
using Servino.Domain.Core.UserAgg.Dtos.Identity;
using Servino.Domain.Core.UserAgg.Entity;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;

namespace Servino.Infa.DataAccess.Repo.EfCore.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<UserProfileDto?> GetProfileByIdAsync(int userId, string role, CancellationToken ct)
    {
        IQueryable<User> query;

        if (role == "Expert")
        {
            query = context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId && !u.IsDeleted)
                .Include(u => u.City)
                .Include(u => u.Expert);
        }
        else if (role == "Customer")
        {
            query = context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId && !u.IsDeleted)
                .Include(u => u.City)
                .Include(u => u.Customer);
        }
        else 
        {
            query = context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId && !u.IsDeleted)
                .Include(u => u.City);
        }

        var user = await query.FirstOrDefaultAsync(ct);
        if (user == null)
            return null;

        var dto = new UserProfileDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            MobileNumber = user.MobileNumber,
            CityId = user.CityId,
            CityName = user.City?.Title,
            ProfileImagePath = user.ProfileImagePath,
            Balance = user.Balance,
            RegisterDate = user.CreatedAt,
            Role = role
        };

        if (role == "Expert" && user.Expert != null)
        {
            dto.ExpertInfo = new ExpertProfileInfo
            {
                Bio = user.Expert.Bio,
                Address = user.Expert.Address,
                BankCardNumber = user.Expert.BankCardNumber,
                ShebaNumber = user.Expert.ShebaNumber,
                AverageScore = user.Expert.AverageScore
            };
        }
        return dto;
    }

    public async Task<bool> UpdateProfileAsync(UpdateProfileDto command, string role, CancellationToken ct)
    {
        try
        {
            var user = await context.Users
                .Include(u => u.Expert)
                .FirstOrDefaultAsync(u => u.Id == command.Id && !u.IsDeleted, ct);

            if (user == null) return false;

            user.FirstName = command.FirstName;
            user.LastName = command.LastName;
            user.CityId = command.CityId;

            if (!string.IsNullOrEmpty(command.ProfileImagePath))
                user.ProfileImagePath = command.ProfileImagePath;

            user.UpdatedAt = DateTime.Now;

            if (role == "Expert" && user.Expert != null)
            {
                user.Expert.Bio = command.Bio;
                user.Expert.Address = command.Address;
                user.Expert.BankCardNumber = command.BankCardNumber;
                user.Expert.ShebaNumber = command.ShebaNumber;
            }

            if (!context.ChangeTracker.HasChanges())
                return true;

            await context.SaveChangesAsync(ct);
            return true;
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
            return false;
        }
    }
    public Task<bool> CityExistsAsync(int cityId, CancellationToken ct)
    {
        throw new NotImplementedException();
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
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == command.Id, ct);
        if (user == null) return false;

        user.FirstName = command.FirstName;
        user.LastName = command.LastName;
        user.MobileNumber = command.Mobile;
        user.CityId = command.CityId;

        if (!string.IsNullOrEmpty(command.ProfileImagePath))
        {
            user.ProfileImagePath = command.ProfileImagePath;
        }

        user.UpdatedAt = DateTime.Now;

        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var affectedRows = await context.Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(u => u.IsDeleted, true)
                .SetProperty(u => u.DeletedAt, DateTime.Now),
                ct);

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


    public async Task<List<UserSummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        var query = context.Users
            .AsNoTracking()
            .Include(u => u.City)
            .Include(u => u.Admin)
            .Include(u => u.Expert)
            .Include(u => u.Customer)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search.SearchKey))
        {
            query = query.Where(u =>
                u.LastName.Contains(search.SearchKey) ||
                u.Email.Contains(search.SearchKey) ||
                u.MobileNumber.Contains(search.SearchKey));
        }


        var pagedUsers = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((search.PageNumber - 1) * search.PageSize)
            .Take(search.PageSize)
            .ToListAsync(ct);

        var result = pagedUsers.Select(u => new UserSummaryDto
        {
            Id = u.Id,
            FullName = $"{u.FirstName} {u.LastName}",
            Email = u.Email,
            Mobile = u.MobileNumber,
            CityName = u.City?.Title ?? "تعیین نشده",
            Balance = u.Balance,
            IsActive = u.IsActive,
            RegisterDate = u.CreatedAt,
            ImageUrl = u.ProfileImagePath!
        }).ToList();

        return result;
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
}
