using Microsoft.Extensions.Logging;
using Servino.Domain.Core._common;
using Servino.Domain.Core.CommentAgg.Contracts.AppService;
using Servino.Domain.Core.CommentAgg.Contracts.Service;
using Servino.Domain.Core.CommentAgg.Dtos;
using Servino.Domain.Core.RequestAgg.Contracts.Service;
using Servino.Domain.Core.RequestAgg.Enum;
using Servino.Domain.Core.SuggestionAgg.Contracts.Service;
using Servino.Framework.Caching;

namespace Servino.Domain.AppService;

public class CommentAppService(ICommentService commentService,IRequestService requestService,
    ISuggestionService suggestionService, ICacheService cache , ILogger<CommentAppService> logger) : ICommentAppService
{
    public async Task<Result<bool>> AddAsync(CreateCommentDto command, CancellationToken ct)
    {
        var request = await requestService.GetByIdAsync(command.RequestId, ct);
        if (request == null) return Result<bool>.Failure("درخواست یافت نشد.");

        if (request.Status != RequestStatus.Paid)
            return Result<bool>.Failure("ثبت نظر فقط پس از پرداخت و اتمام نهایی کار امکان‌پذیر است.");

        if (request.CustomerId != command.CustomerId)
            return Result<bool>.Failure("شما مالک این درخواست نیستید.");

        var exists = await commentService.ExistsByRequestIdAndCustomerIdAsync(command.RequestId, command.CustomerId, ct);
        if (exists)
            return Result<bool>.Failure("شما قبلاً برای این سفارش نظر ثبت کرده‌اید.");

        var suggestion = await suggestionService.GetByIdAsync(request.WinnerSuggestionId.Value, ct);
        if (suggestion.ExpertId != command.ExpertId)
            return Result<bool>.Failure("شما فقط می‌توانید برای متخصص انجام‌دهنده کار نظر دهید.");

        var result = await commentService.AddAsync(command, ct);

        if (result)
        {
            await cache.RemoveAsync(CacheKeys.CommentByRequestAndCustomer(command.RequestId, command.CustomerId), ct);
            await cache.BumpStampAsync(CacheKeys.StampAllRequests, CacheTtl.Stamps, ct);
            await cache.BumpStampAsync(CacheKeys.StampAvailableRequests, CacheTtl.Stamps, ct);

        }

        return result
            ? Result<bool>.Success(true, "نظر شما ثبت شد و پس از تایید نمایش داده می‌شود.")
            : Result<bool>.Failure("خطا در ثبت نظر.");
    }


    public async Task<Result<List<CommentDto>>> GetAllAsync(PaginationRequestDto pagination, CancellationToken ct)
    {
        var key = CacheKeys.CommentsAll(pagination.SearchKey ?? "", pagination.PageNumber, pagination.PageSize);

        return await cache.GetOrSetAsync(key,
            async () => await commentService.GetAllAsync(pagination, ct), CacheTtl.Comments, ct);
    }


    public async Task<Result<List<CommentDto>>> GetApprovedByExpertIdAsync(int expertId, PaginationRequestDto pagination, CancellationToken ct)
    {
        var key = CacheKeys.CommentsApprovedByExpertId(expertId);

        return await cache.GetOrSetAsync(key,
            async () => await commentService.GetApprovedByExpertIdAsync(expertId, pagination, ct), CacheTtl.Comments, ct);
    }


    public async Task<Result<CommentDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        try
        {
            var key = CacheKeys.CommentDetails(id);
            var comment = await cache.GetOrSetAsync(key,
                async () => await commentService.GetByIdAsync(id, ct), CacheTtl.CommentDetails, ct);

            return comment == null
                ? Result<CommentDto>.Failure("دیدگاه مورد نظر یافت نشد.", "404")
                : Result<CommentDto>.Success(comment);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "System error in CommentAppService.GetByIdAsync | CommentId: {CommentId}", id);
            return Result<CommentDto>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }


    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken ct)
    {
        var isDeleted = await commentService.DeleteAsync(id, ct);

        if (isDeleted)
        {
            await cache.RemoveAsync(CacheKeys.CommentDetails(id), ct);
            await cache.BumpStampAsync(CacheKeys.StampAllRequests, CacheTtl.Stamps, ct);
            await cache.BumpStampAsync(CacheKeys.StampAvailableRequests, CacheTtl.Stamps, ct);

        }

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

        await cache.RemoveAsync(CacheKeys.CommentDetails(id), ct);
        await cache.BumpStampAsync(CacheKeys.StampAllRequests, CacheTtl.Stamps, ct);
        await cache.BumpStampAsync(CacheKeys.StampAvailableRequests, CacheTtl.Stamps, ct);

        return Result<bool>.Success(true, $"دیدگاه با موفقیت {statusMessage} شد.");
    }

    public async Task<Result<CommentDto>> GetMyCommentForRequestAsync(int requestId, int customerId, CancellationToken ct)
    {
        var key = CacheKeys.CommentByRequestAndCustomer(requestId, customerId);

        var comment = await cache.GetOrSetAsync(key,
            async () => await commentService.GetByRequestIdAndCustomerIdAsync(requestId, customerId, ct),
            CacheTtl.CommentDetails, ct);

        return comment == null
            ? Result<CommentDto>.Failure("دیدگاهی برای این درخواست یافت نشد.", "404")
            : Result<CommentDto>.Success(comment);
    }






    private static class CacheKeys
    {
        public static string CommentDetails(int id) => $"comment:details:{id}";
        public static string CommentsAll(string searchKey, int pageNumber, int pageSize) =>
            $"comments:all:{searchKey}:{pageNumber}:{pageSize}";
        public static string CommentsApprovedByExpertId(int expertId) => $"comments:approved:expert:{expertId}";
        public static string CommentByRequestAndCustomer(int requestId, int customerId) => $"comment:request:{requestId}:customer:{customerId}";

        public static string StampAllRequests => "stamp:requests:all"; 
        public static string StampAvailableRequests => "stamp:requests:available"; 
    }


    private static class CacheTtl
    {
        public static readonly TimeSpan CommentDetails = TimeSpan.FromMinutes(5);
        public static readonly TimeSpan Comments = TimeSpan.FromMinutes(10);
        public static readonly TimeSpan Stamps = TimeSpan.FromHours(6);  

    }


}