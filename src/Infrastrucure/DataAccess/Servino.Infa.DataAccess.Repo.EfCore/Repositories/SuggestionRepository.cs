using Microsoft.EntityFrameworkCore;
using Servino.Domain.Core.SuggestionAgg.Contracts.Data;
using Servino.Domain.Core.SuggestionAgg.Dtos;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;

namespace Servino.Infa.DataAccess.Repo.EfCore.Repositories;

public class SuggestionRepository(AppDbContext context) : ISuggestionRepository
{
    public async Task<List<SuggestionSummaryDto>> GetByRequestIdAsync(int requestId, CancellationToken ct)
    {
        return await context.Suggestions
            .AsNoTracking()
            .Where(s => s.RequestId == requestId)
            .Include(s => s.Expert)
            .ThenInclude(e => e.User)
            .OrderByDescending(s => s.CreatedAt) 
            .Select(s => new SuggestionSummaryDto
            {
                Id = s.Id,
                ExpertId = s.ExpertId,
                ExpertFullName = $"{s.Expert.User.FirstName} {s.Expert.User.LastName}",
                ExpertMobile = s.Expert.User.MobileNumber,
                SuggestedPrice = s.SuggestedPrice,
                SuggestedDate = s.SuggestedDate,
                EstimatedDurationHours = s.EstimatedDurationHours,
                Note = s.Note,
                Status = s.Status,
                CreatedAt = s.CreatedAt
            })
            .ToListAsync(ct);
    }
}