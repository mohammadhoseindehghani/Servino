using app.Application.Common;
using app.Application.DTOs.CategoryDTOs;
using MediatR;

namespace app.Application.Features.Categories.Commands.Create;

public record CreateCategoryCommand(string Title, string? ImagePath, int? ParentId)
    : IRequest<Result<bool>>;