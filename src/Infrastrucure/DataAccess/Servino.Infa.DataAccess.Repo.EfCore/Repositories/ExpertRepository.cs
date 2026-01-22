using Microsoft.EntityFrameworkCore;
using Servino.Domain.Core.UserAgg.Contracts.Data;
using Servino.Domain.Core.UserAgg.Dtos;
using Servino.Domain.Core.UserAgg.Entity;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;

namespace Servino.Infa.DataAccess.Repo.EfCore.Repositories;

public class ExpertRepository(AppDbContext context) : IExpertRepository
{
    public async Task<bool> CreateAsync(int userId, CancellationToken ct)
    {
        var expert = new Expert { UserId = userId };
        context.Experts.Add(expert);
        return await context.SaveChangesAsync(ct)>0;
    }

    public async Task<int> GetIdByUserIdAsync(int userId, CancellationToken ct)
    {
        return await context.Experts
            .Where(e => e.UserId == userId)
            .Select(e => e.Id)
            .FirstOrDefaultAsync(ct); 
    }

    public async Task<ExpertProfileDto?> GetByUserIdAsync(int userId, CancellationToken ct)
    {
        return await context.Experts.Where(x => x.UserId == userId)
            .Select(e => new ExpertProfileDto()
                {
                    UserId = e.UserId,
                    ExpertId = e.Id,
                    Address = e.Address,
                    CityId = e.User.CityId,
                    FirstName = e.User.FirstName,
                    LastName = e.User.LastName,
                    BankCardNumber = e.BankCardNumber,
                    Bio = e.Bio,
                    Email = e.User.Email,
                    Phone = e.User.MobileNumber,
                }
            ).FirstOrDefaultAsync(ct);
    }


    //add rollback or Stored Procedure 
    public async Task<bool> UpdateProfile(UpdateExpertProfileDto dto, CancellationToken ct)
    {
        var expertRows = await context.Experts
            .Where(e => e.UserId == dto.UserId)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(e => e.Address, e => dto.Address ?? e.Address)
                    .SetProperty(e => e.BankCardNumber, e => dto.BankCardNumber ?? e.BankCardNumber)
                    .SetProperty(e => e.ShebaNumber, e => dto.ShebaNumber ?? e.ShebaNumber)
                    .SetProperty(e => e.Bio, e => dto.Bio ?? e.Bio)
                    .SetProperty(e => e.UpdatedAt, DateTime.UtcNow),
                ct);

        var userRows = await context.Users
            .Where(u => u.Id == dto.UserId)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.FirstName, u => dto.FirstName ?? u.FirstName)
                    .SetProperty(u => u.LastName, u => dto.LastName ?? u.LastName)
                    .SetProperty(u => u.CityId, u => dto.CityId ?? u.CityId)
                    .SetProperty(u => u.ProfileImagePath,
                        u => dto.ProfileImagePath ?? u.ProfileImagePath)
                    .SetProperty(u => u.UpdatedAt, DateTime.UtcNow),
                ct);

        return expertRows > 0 && userRows > 0;
    }


    //public async Task<bool> UpdateProfile(UpdateExpertProfileDto command, CancellationToken ct)
    //{
    //    var expert = await context.Experts.Include(e => e.User)
    //        .FirstOrDefaultAsync(e => e.UserId == command.UserId, ct);

    //    if (expert == null) return false;

    //    expert.Address = command.Address ?? expert.Address;
    //    expert.BankCardNumber = command.BankCardNumber ?? expert.BankCardNumber;
    //    expert.ShebaNumber = command.ShebaNumber ?? expert.ShebaNumber;
    //    expert.Bio = command.Bio ?? expert.Bio;

    //    expert.User.FirstName = command.FirstName ?? expert.User.FirstName;
    //    expert.User.LastName = command.LastName ?? expert.User.LastName;
    //    expert.User.CityId = command.CityId ?? expert.User.CityId;
    //    expert.User.ProfileImagePath = command.ProfileImagePath ?? expert.User.ProfileImagePath;
    //    await context.SaveChangesAsync(ct);
    //    return true;
    //}
}