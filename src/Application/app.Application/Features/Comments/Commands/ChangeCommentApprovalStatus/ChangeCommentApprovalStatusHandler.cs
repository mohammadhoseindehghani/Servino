using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.CommentAgg;
using MediatR;

namespace app.Application.Features.Comments.Commands.ChangeCommentApprovalStatus;

public class ChangeCommentApprovalStatusHandler(ICommentService commentService)
    : IRequestHandler<ChangeCommentApprovalStatusCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(ChangeCommentApprovalStatusCommand request, CancellationToken ct)
    {
        var isUpdated = await commentService.ChangeApprovalStatusAsync(request.Id, request.IsApproved, ct);
        if (!isUpdated)
            return Result<bool>.Failure("تغییر وضعیت انجام نشد.", "Update_Error");

        var statusMessage = request.IsApproved ? "تایید" : "رد";
        return Result<bool>.Success(true, $"دیدگاه با موفقیت {statusMessage} شد.");
    }
}