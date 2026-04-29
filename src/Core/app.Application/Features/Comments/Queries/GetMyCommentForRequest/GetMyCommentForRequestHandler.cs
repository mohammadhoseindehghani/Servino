using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.DTOs.CommentDTOs;
using MediatR;

namespace app.Application.Features.Comments.Queries.GetMyCommentForRequest;

public class GetMyCommentForRequestHandler(ICommentRepository commentRepository)
    : IRequestHandler<GetMyCommentForRequestQuery, Result<CommentDto>>
{

    public async Task<Result<CommentDto>> Handle(GetMyCommentForRequestQuery request, CancellationToken ct)
    {
        var comment = await commentRepository.GetByRequestIdAndCustomerIdAsync(request.RequestId, request.CustomerId, ct);
        return comment == null
            ? Result<CommentDto>.Failure("دیدگاهی یافت نشد.", "404")
            : Result<CommentDto>.Success(comment);
    }
}