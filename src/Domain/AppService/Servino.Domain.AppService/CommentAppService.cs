using Servino.Domain.Core._common;
using Servino.Domain.Core.CommentAgg.Contracts.AppService;
using Servino.Domain.Core.CommentAgg.Contracts.Service;
using Servino.Domain.Core.CommentAgg.Dtos;
using Servino.Domain.Core.RequestAgg.Contracts.Service;

namespace Servino.Domain.AppService;

public class CommentAppService(ICommentService commentService,IRequestService requestService) : ICommentAppService
{
    public async Task<Result<bool>> AddAsync(CreateCommentDto command, CancellationToken ct)
    {
        
        if (string.IsNullOrWhiteSpace(command.Text) || string.IsNullOrWhiteSpace(command.Title)
            || command.CustomerId == 0 || command.ExpertId == 0 || command.RequestId == 0)
        {
            return Result<bool>.Failure("پر کردن تمام مقادیر الزامی است.");
        }

        var isCreated = await commentService.AddAsync(command, ct);

        return !isCreated ? Result<bool>.Failure("خطایی در ثبت دیدگاه رخ داده است.", "Create_Error") 
            : Result<bool>.Success(true, "دیدگاه شما با موفقیت ثبت شد و پس از تایید نمایش داده می‌شود.");
    }

    public async Task<Result<List<CommentDto>>> GetAllAsync(PaginationRequestDto pagination, CancellationToken ct)
    {
        var comments = await commentService.GetAllAsync(pagination, ct);
        return Result<List<CommentDto>>.Success(comments);
    }

    public async Task<Result<List<CommentDto>>> GetApprovedByExpertIdAsync(int expertId, PaginationRequestDto pagination, CancellationToken ct)
    {
        var comments = await commentService.GetApprovedByExpertIdAsync(expertId, pagination, ct);
        return Result<List<CommentDto>>.Success(comments);
    }

    public async Task<Result<CommentDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var comment = await commentService.GetByIdAsync(id, ct);

        return comment is null ? Result<CommentDto>.Failure("دیدگاه مورد نظر یافت نشد.", "404") 
            : Result<CommentDto>.Success(comment);
    }

    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken ct)
    {
        var isDeleted = await commentService.DeleteAsync(id, ct);

        return !isDeleted ? Result<bool>.Failure("دیدگاه یافت نشد یا حذف نشد.", "Delete_Error") 
            : Result<bool>.Success(true, "دیدگاه با موفقیت حذف شد.");
    }

    public async Task<Result<bool>> ChangeApprovalStatusAsync(int id, bool isApproved, CancellationToken ct)
    {
        var isUpdated = await commentService.ChangeApprovalStatusAsync(id, isApproved, ct);

        if (!isUpdated)
        {
            return Result<bool>.Failure("تغییر وضعیت انجام نشد. ممکن است دیدگاه یافت نشده باشد.", "Update_Error");
        }

        var statusMessage = isApproved ? "تایید" : "رد";
        return Result<bool>.Success(true, $"دیدگاه با موفقیت {statusMessage} شد.");
    }
}