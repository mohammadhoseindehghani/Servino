using app.Application.Common;
using app.Application.Contracts.Repositories;
using MediatR;

namespace app.Application.Features.Comments.Commands.DeleteComment;

public class DeleteCommentCommandHandler(ICommentRepository commentRepository)
    : IRequestHandler<DeleteCommentCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(DeleteCommentCommand request, CancellationToken ct)
    {
        var isDeleted = await commentRepository.DeleteAsync(request.Id, ct);
        return !isDeleted
            ? Result<bool>.Failure("دیدگاه یافت نشد یا حذف نشد.", "Delete_Error")
            : Result<bool>.Success(true, "دیدگاه با موفقیت حذف شد.");
    }
}