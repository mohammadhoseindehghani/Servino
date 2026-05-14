using app.Application.Contracts.Common;
using MediatR;

namespace app.Application.Features.Comments.Commands.CreateComment;

public record CreateCommentCommand(
    int RequestId,
    int CustomerId,
    int ExpertId,
    string Title,
    string Description,
    int Score) : IRequest<Result<bool>>;
