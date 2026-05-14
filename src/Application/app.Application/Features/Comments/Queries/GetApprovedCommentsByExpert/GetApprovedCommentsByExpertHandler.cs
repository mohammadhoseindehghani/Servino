using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.CommentAgg;
using app.Application.Contracts.DTOs.CommentDTOs;
using MediatR;

namespace app.Application.Features.Comments.Queries.GetApprovedCommentsByExpert;

public class GetApprovedCommentsByExpertHandler(ICommentService commentService)
    : IRequestHandler<GetApprovedCommentsByExpertQuery, Result<List<CommentDto>>>
{

    public async Task<Result<List<CommentDto>>> Handle(GetApprovedCommentsByExpertQuery request, CancellationToken ct)
    {
        return await commentService.GetApprovedByExpertIdAsync(request.ExpertId, request.Pagination, ct);
    }
}