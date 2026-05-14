using app.Application.Contracts.Common;
using MediatR;

namespace app.Application.Features.Comments.Commands.ChangeCommentApprovalStatus;

public record ChangeCommentApprovalStatusCommand(int Id, bool IsApproved) : IRequest<Result<bool>>;
