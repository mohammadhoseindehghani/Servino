using app.Application.Common;
using app.Application.DTOs.CommentDTOs;
using MediatR;

namespace app.Application.Features.Comments.Queries.GetCommentById;

public record GetCommentByIdQuery(int Id) : IRequest<Result<CommentDto>>;
