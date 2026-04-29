using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.DTOs.CommentDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Comments.Queries.GetCommentById;

public class GetCommentByIdQueryHandler(ICommentRepository commentRepository, ILogger<GetCommentByIdQueryHandler> logger)
    : IRequestHandler<GetCommentByIdQuery, Result<CommentDto>>
{

    public async Task<Result<CommentDto>> Handle(GetCommentByIdQuery request, CancellationToken ct)
    {
        try
        {
            var comment = await commentRepository.GetByIdAsync(request.Id, ct);
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