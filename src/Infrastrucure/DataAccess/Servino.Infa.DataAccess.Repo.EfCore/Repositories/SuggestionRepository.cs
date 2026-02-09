using Microsoft.EntityFrameworkCore;
using Servino.Domain.Core.SuggestionAgg.Contracts.Data;
using Servino.Domain.Core.SuggestionAgg.Dtos;
using Servino.Domain.Core.SuggestionAgg.Entity;
using Servino.Domain.Core.SuggestionAgg.Enum;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;

namespace Servino.Infa.DataAccess.Repo.EfCore.Repositories;

public class SuggestionRepository(AppDbContext context) : ISuggestionRepository
{
    public async Task<bool> CreateAsync(CreateSuggestionDto command, CancellationToken ct)
    {
        var suggestion = new Suggestion()
        {
            SuggestedPrice = command.SuggestedPrice,
            SuggestedDate = command.SuggestedDate,
            EstimatedDurationHours = command.EstimatedDurationHours,
            RequestId = command.RequestId,
            ExpertId = command.ExpertId,
            Note = command.Note,
            Status = SuggestionStatus.Pending
        };
        context.Add(suggestion);
        return await context.SaveChangesAsync(ct) > 0;
    }

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

    public async Task<SuggestionDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await context.Suggestions
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new SuggestionDto
            {
                Id = s.Id,
                RequestId = s.RequestId,
                ExpertId = s.ExpertId,
                ExpertUserId = s.Expert.UserId,
                SuggestedPrice = s.SuggestedPrice,
                SuggestedDate = s.SuggestedDate,
                EstimatedDurationHours = s.EstimatedDurationHours,
                Note = s.Note,
                Status = s.Status,
                CreatedAt = s.CreatedAt
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<bool> UpdateAsync(UpdateSuggestionDto command, CancellationToken ct)
    {
        var suggestion = await context.Suggestions.FirstOrDefaultAsync(s => s.Id == command.Id, ct);

        if (suggestion == null) return false;

        suggestion.Status = command.Status;
        suggestion.SuggestedPrice = command.SuggestedPrice;
        suggestion.SuggestedDate = command.SuggestedDate;
        suggestion.EstimatedDurationHours = command.EstimatedDurationHours;
        suggestion.Note = command.Note;
        suggestion.UpdatedAt = DateTime.Now;

        return await context.SaveChangesAsync(ct) > 0;
    }
}