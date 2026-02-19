using Servino.Domain.Core._common;
using Servino.Domain.Core.CommentAgg.Dtos;

namespace Servino.Domain.Core.CommentAgg.Contracts.AppService;

public interface ICommentAppService
{
    Task<Result<bool>> AddAsync(CreateCommentDto command, CancellationToken ct);
    Task<Result<List<CommentDto>>> GetAllAsync(PaginationRequestDto pagination, CancellationToken ct);
    Task<Result<List<CommentDto>>> GetApprovedByExpertIdAsync(int expertId, PaginationRequestDto pagination, CancellationToken ct);
    Task<Result<CommentDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, CancellationToken ct);
    Task<Result<bool>> ChangeApprovalStatusAsync(int id, bool isApproved, CancellationToken ct);


    Task<Result<CommentDto>> GetMyCommentForRequestAsync(int requestId, int customerId, CancellationToken ct);

}