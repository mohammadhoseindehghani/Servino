using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.CommentDTOs;
using MediatR;

namespace app.Application.Features.Comments.Queries.GetMyCommentForRequest;

public record GetMyCommentForRequestQuery(int RequestId, int CustomerId) : IRequest<Result<CommentDto>>;
