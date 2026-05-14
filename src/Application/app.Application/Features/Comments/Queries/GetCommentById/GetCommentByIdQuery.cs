using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.CommentDTOs;
using MediatR;

namespace app.Application.Features.Comments.Queries.GetCommentById;

public record GetCommentByIdQuery(int Id) : IRequest<Result<CommentDto>>;
