using Microsoft.EntityFrameworkCore;
using Servino.Domain.Core._common;
using Servino.Domain.Core.CommentAgg.Contracts.Data;
using Servino.Domain.Core.CommentAgg.Dtos;
using Servino.Domain.Core.CommentAgg.Entity;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;

namespace Servino.Infa.DataAccess.Repo.EfCore.Repositories;

public class CommentRepository(AppDbContext context) : ICommentRepository
{
    public async Task<bool> AddAsync(CreateCommentDto command, CancellationToken ct)
    {
        try
        {
            var comment = new Comment
            {
                Title = command.Title,
                Text = command.Text,
                Rating = command.Rating,
                CustomerId = command.CustomerId,
                ExpertId = command.ExpertId,
                RequestId = command.RequestId,
                IsApproved = false,
                CreatedAt = DateTime.Now
            };

            await context.Comments.AddAsync(comment, ct);
            await context.SaveChangesAsync(ct);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<CommentDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken ct)
    {
        var query = context.Comments.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.SearchKey))
        {
            query = query.Where(c => c.Title.Contains(pagination.SearchKey) ||
                                     c.Text.Contains(pagination.SearchKey));
        }

        return await query
            .Include(c => c.Customer)
            .Include(c => c.Expert)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                Title = c.Title,
                Text = c.Text,
                Rating = c.Rating,
                IsApproved = c.IsApproved,
                CreatedAt = c.CreatedAt,
                CustomerName = c.Customer != null ? $"{c.Customer.User.FirstName} {c.Customer.User.LastName}" : "Unknown",
                ExpertName = c.Expert != null ? $"{c.Expert.User.FirstName} {c.Expert.User.LastName}" : "Unknown"
            })
            .ToListAsync(ct);
    }

    public async Task<List<CommentDto>> GetApprovedByExpertIdAsync(int expertId, PaginationRequestDto pagination, CancellationToken ct)
    {
        var query = context.Comments
            .AsNoTracking()
            .Where(c => c.ExpertId == expertId && c.IsApproved);

        if (!string.IsNullOrWhiteSpace(pagination.SearchKey))
        {
            query = query.Where(c => c.Title.Contains(pagination.SearchKey) ||
                                     c.Text.Contains(pagination.SearchKey));
        }

        return await query
            .Include(c => c.Customer)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                Title = c.Title,
                Text = c.Text,
                Rating = c.Rating,
                IsApproved = c.IsApproved,
                CreatedAt = c.CreatedAt,
                CustomerName = c.Customer != null ? $"{c.Customer.User.FirstName} {c.Customer.User.LastName}" : "Unknown",
                ExpertName = c.Expert != null ? $"{c.Expert.User.FirstName} {c.Expert.User.LastName}" : "Unknown"
            })
            .ToListAsync(ct);
    }

    public async Task<CommentDto> GetByIdAsync(int id, CancellationToken ct)
    {
        return await context.Comments
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Include(c => c.Customer)
            .Include(c => c.Expert)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                Title = c.Title,
                Text = c.Text,
                Rating = c.Rating,
                IsApproved = c.IsApproved,
                CreatedAt = c.CreatedAt,
                CustomerName = c.Customer != null ? $"{c.Customer.User.FirstName} {c.Customer.User.LastName}" : "Unknown",
                ExpertName = c.Expert != null ? $"{c.Expert.User.FirstName} {c.Expert.User.LastName}" : "Unknown"
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var comment = await context.Comments.FindAsync([id], cancellationToken: ct);

        if (comment is null)
            return false;

        context.Comments.Remove(comment);
        await context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> ChangeApprovalStatusAsync(int id, bool isApproved, CancellationToken ct)
    {
        var comment = await context.Comments.FindAsync([id], cancellationToken: ct);

        if (comment is null)
            return false;

        comment.IsApproved = isApproved;

        await context.SaveChangesAsync(ct);
        return true;
    }
}