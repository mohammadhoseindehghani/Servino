using app.Application.Contracts.Common;
using MediatR;

namespace app.Application.Features.Comments.Commands.DeleteComment;

public record DeleteCommentCommand(int Id) : IRequest<Result<bool>>;
