using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.CommentAgg;
using app.Application.Contracts.DTOs.CommentDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Comments.Queries.GetCommentById;

public class GetCommentByIdQueryHandler(ICommentService commentService, ILogger<GetCommentByIdQueryHandler> logger)
    : IRequestHandler<GetCommentByIdQuery, Result<CommentDto>>
{

    public async Task<Result<CommentDto>> Handle(GetCommentByIdQuery request, CancellationToken ct)
    {
        try
        {
            var comment = await commentService.GetByIdAsync(request.Id, ct);
            return comment == null
                ? Result<CommentDto>.Failure("دیدگاه مورد نظر یافت نشد.", "404")
                : Result<CommentDto>.Success(comment);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in GetCommentByIdQuery | Id: {Id}", request.Id);
            return Result<CommentDto>.Failure("خطای سیستمی رخ داده است.");
        }
    }
}