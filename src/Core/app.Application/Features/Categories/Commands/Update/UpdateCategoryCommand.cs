using app.Application.Common;
using MediatR;

namespace app.Application.Features.Categories.Commands.Update;

public record UpdateCategoryCommand(int Id, string Title, int? ParentId)
    : IRequest<Result<bool>>;
