using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Services.CommentAgg;
using app.Application.Contracts.DTOs.CommentDTOs;
using MediatR;

namespace app.Application.Features.Comments.Queries.GetMyCommentForRequest;

public class GetMyCommentForRequestHandler(ICommentService commentService)
    : IRequestHandler<GetMyCommentForRequestQuery, Result<CommentDto>>
{

    public async Task<Result<CommentDto>> Handle(GetMyCommentForRequestQuery request, CancellationToken ct)
    {
        var comment = await commentService.GetByRequestIdAndCustomerIdAsync(request.RequestId, request.CustomerId, ct);
        return comment == null
            ? Result<CommentDto>.Failure("دیدگاهی یافت نشد.", "404")
            : Result<CommentDto>.Success(comment);
    }
}