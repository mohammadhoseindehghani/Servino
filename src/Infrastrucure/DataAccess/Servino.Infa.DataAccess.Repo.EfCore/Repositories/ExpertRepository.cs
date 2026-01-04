using Microsoft.EntityFrameworkCore;
using Servino.Domain.Core.UserAgg.Contracts.Data;
using Servino.Domain.Core.UserAgg.Dtos;
using Servino.Domain.Core.UserAgg.Entity;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;

namespace Servino.Infa.DataAccess.Repo.EfCore.Repositories;

public class ExpertRepository(AppDbContext context) : IExpertRepository
{
    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await context.SaveChangesAsync(ct);
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

    public async Task<bool> UpdateProfile(UpdateExpertProfileDto command, CancellationToken ct)
    {
        var expert = await context.Experts.Include(e => e.User)
            .FirstOrDefaultAsync(e => e.UserId == command.UserId, ct);

        if (expert == null) return false;

        expert.Address = command.Address ?? expert.Address;
        expert.BankCardNumber = command.BankCardNumber ?? expert.BankCardNumber;
        expert.ShebaNumber = command.ShebaNumber ?? expert.ShebaNumber;
        expert.Bio = command.Bio ?? expert.Bio;

        expert.User.FirstName = command.FirstName ?? expert.User.FirstName;
        expert.User.LastName = command.LastName ?? expert.User.LastName;
        expert.User.CityId = command.CityId ?? expert.User.CityId;
        expert.User.ProfileImagePath = command.ProfileImagePath ?? expert.User.ProfileImagePath;
        await context.SaveChangesAsync(ct);
        return true;
    }
}