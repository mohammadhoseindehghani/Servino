using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.DTOs.CommentDTOs;
using app.Domain.CommentAgg.Entities;
using Microsoft.EntityFrameworkCore;

namespace app.Persistence.Repositories;

public class CommentRepository(AppDbContext context) : ICommentRepository
{
    public async Task<bool> AddAsync(CreateCommentDto command, CancellationToken ct)
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

            context.Comments.Add(comment);
            return await context.SaveChangesAsync(ct) > 0;
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
                CustomerName = c.Customer == null ? null : c.Customer.User.FirstName + " " + c.Customer.User.LastName,
                ExpertName = c.Expert == null ? null : c.Expert.User.FirstName + " " + c.Expert.User.LastName
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
                CustomerName = c.Customer == null ? null : c.Customer.User.FirstName + " " + c.Customer.User.LastName,
                ExpertName = c.Expert == null ? null : c.Expert.User.FirstName + " " + c.Expert.User.LastName
            })
            .ToListAsync(ct);
    }

    public async Task<CommentDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await context.Comments
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                Title = c.Title,
                Text = c.Text,
                Rating = c.Rating,
                IsApproved = c.IsApproved,
                CreatedAt = c.CreatedAt,
                CustomerName = c.Customer == null ? null : c.Customer.User.FirstName + " " + c.Customer.User.LastName,
                ExpertName = c.Expert == null ? null : c.Expert.User.FirstName + " " + c.Expert.User.LastName
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var affectedRows = await context.Comments
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync(ct);

        return affectedRows > 0;
    }

    public async Task<bool> ChangeApprovalStatusAsync(int id, bool isApproved, CancellationToken ct)
    {
        var affectedRows = await context.Comments
            .Where(c => c.Id == id)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(c => c.IsApproved, isApproved)
                    .SetProperty(c => c.UpdatedAt, DateTime.UtcNow),
                ct);

        return affectedRows > 0;
    }

    public async Task<CommentDto?> GetByRequestIdAndCustomerIdAsync(int requestId, int customerId, CancellationToken ct)
    {
        return await context.Comments
            .AsNoTracking()
            .Where(c => c.RequestId == requestId && c.CustomerId == customerId)
            .Include(c => c.Customer)
            .Include(c => c.Expert)
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                Title = c.Title,
                Text = c.Text,
                Rating = c.Rating,
                IsApproved = c.IsApproved,
                CreatedAt = c.CreatedAt,
                CustomerName = c.Customer == null ? null : c.Customer.User.FirstName + " " + c.Customer.User.LastName,
                ExpertName = c.Expert == null ? null : c.Expert.User.FirstName + " " + c.Expert.User.LastName
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<bool> ExistsByRequestIdAndCustomerIdAsync(int requestId, int customerId, CancellationToken ct)
    {
        return await context.Comments
            .AsNoTracking()
            .AnyAsync(c => c.RequestId == requestId && c.CustomerId == customerId, ct);
    }

}