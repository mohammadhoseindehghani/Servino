using Servino.Domain.Core._common;
using Servino.Domain.Core.CommentAgg.Contracts.Data;
using Servino.Domain.Core.CommentAgg.Contracts.Service;
using Servino.Domain.Core.CommentAgg.Dtos;

namespace Servino.Domain.Service;

public class CommentService(ICommentRepository commentRepo) : ICommentService
{
    public async Task<bool> AddAsync(CreateCommentDto command, CancellationToken ct)
    {
        return await commentRepo.AddAsync(command, ct);
    }

    public async Task<List<CommentDto>> GetAllAsync(PaginationRequestDto pagination, CancellationToken ct)
    {
        return await commentRepo.GetAllAsync(pagination, ct);
    }

    public async Task<List<CommentDto>> GetApprovedByExpertIdAsync(int expertId, PaginationRequestDto pagination, CancellationToken ct)
    {
        return await commentRepo.GetApprovedByExpertIdAsync(expertId, pagination, ct);
    }


    public async Task<CommentDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await commentRepo.GetByIdAsync(id, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        return await commentRepo.DeleteAsync(id, ct);
    }

    public async Task<bool> ChangeApprovalStatusAsync(int id, bool isApproved, CancellationToken ct)
    {
        return await commentRepo.ChangeApprovalStatusAsync(id, isApproved, ct);
    }
}