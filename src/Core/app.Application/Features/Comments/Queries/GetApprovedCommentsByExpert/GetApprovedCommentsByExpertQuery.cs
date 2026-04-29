using app.Application.Common;
using app.Application.DTOs.CommentDTOs;
using MediatR;

namespace app.Application.Features.Comments.Queries.GetApprovedCommentsByExpert;

public record GetApprovedCommentsByExpertQuery(int ExpertId, PaginationRequestDto Pagination) : IRequest<Result<List<CommentDto>>>;
