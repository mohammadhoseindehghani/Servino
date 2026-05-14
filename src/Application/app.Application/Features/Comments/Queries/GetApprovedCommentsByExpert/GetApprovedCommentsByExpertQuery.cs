using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.CommentDTOs;
using MediatR;

namespace app.Application.Features.Comments.Queries.GetApprovedCommentsByExpert;

public record GetApprovedCommentsByExpertQuery(int ExpertId, PaginationRequestDto Pagination) : IRequest<Result<List<CommentDto>>>;
