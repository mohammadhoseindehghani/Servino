using app.Application.Common;
using MediatR;

namespace app.Application.Features.Categories.Commands.Update;

public record UpdateCategoryCommand(int Id, string Title, string? ImagePath, int? ParentId)
    : IRequest<Result<bool>>;
