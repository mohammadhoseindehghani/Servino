using app.Application.Common;
using MediatR;

namespace app.Application.Features.Categories.Commands.Create;

public record CreateCategoryCommand(string Title, int? ParentId)
    : IRequest<Result<bool>>;