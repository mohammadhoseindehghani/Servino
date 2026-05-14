using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using MediatR;

namespace app.Application.Features.Comments.Commands.ChangeCommentApprovalStatus;

public class ChangeCommentApprovalStatusHandler(ICommentRepository commentRepository)
    : IRequestHandler<ChangeCommentApprovalStatusCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(ChangeCommentApprovalStatusCommand request, CancellationToken ct)
    {
        var isUpdated = await commentRepository.ChangeApprovalStatusAsync(request.Id, request.IsApproved, ct);
        if (!isUpdated)
            return Result<bool>.Failure("تغییر وضعیت انجام نشد.", "Update_Error");

        var statusMessage = request.IsApproved ? "تایید" : "رد";
        return Result<bool>.Success(true, $"دیدگاه با موفقیت {statusMessage} شد.");
    }
}