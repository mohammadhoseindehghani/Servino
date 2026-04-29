using app.Application.Common;
using app.Application.DTOs.CategoryDTOs;
using MediatR;

namespace app.Application.Features.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQuery(int Id) : IRequest<Result<CategoryDto>>;