using app.Application.Common;
using app.Application.DTOs.CommentDTOs;

namespace app.Application.Contracts.Repositories;

public interface ICommentRepository
{
    Task<bool> AddAsync(CreateCommentDto command, CancellationToken ct);
    Task<List<CommentDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken ct);
    Task<List<CommentDto>> GetApprovedByExpertIdAsync(int expertId, PaginationRequestDto pagination, CancellationToken ct);
    Task<CommentDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> ChangeApprovalStatusAsync(int id, bool isApproved, CancellationToken ct);

    Task<CommentDto?> GetByRequestIdAndCustomerIdAsync(int requestId, int customerId, CancellationToken ct);
    Task<bool> ExistsByRequestIdAndCustomerIdAsync(int requestId, int customerId, CancellationToken ct);
}
