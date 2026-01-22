using Servino.Domain.Core._common;
using Servino.Domain.Core.CommentAgg.Dtos;

namespace Servino.Domain.Core.CommentAgg.Contracts.Data;

public interface ICommentRepository
{
    Task<bool> AddAsync(CreateCommentDto command, CancellationToken ct);
    Task<List<CommentDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken ct);
    Task<List<CommentDto>> GetApprovedByExpertIdAsync(int expertId, PaginationRequestDto pagination, CancellationToken ct);
    Task<CommentDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> ChangeApprovalStatusAsync(int id, bool isApproved, CancellationToken ct);
}
