using app.Application.Common;
using app.Application.DTOs.CommentDTOs;
using MediatR;

namespace app.Application.Features.Comments.Queries.GetMyCommentForRequest;

public record GetMyCommentForRequestQuery(int RequestId, int CustomerId) : IRequest<Result<CommentDto>>;
