using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.CommentAgg;
using MediatR;

namespace app.Application.Features.Comments.Commands.DeleteComment;

public class DeleteCommentCommandHandler(ICommentService commentService)
    : IRequestHandler<DeleteCommentCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(DeleteCommentCommand request, CancellationToken ct)
    {
        var isDeleted = await commentService.DeleteAsync(request.Id, ct);
        return !isDeleted
            ? Result<bool>.Failure("دیدگاه یافت نشد یا حذف نشد.", "Delete_Error")
            : Result<bool>.Success(true, "دیدگاه با موفقیت حذف شد.");
    }
}