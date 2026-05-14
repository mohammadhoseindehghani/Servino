using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.CategoryDTOs;
using MediatR;

namespace app.Application.Features.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQuery(int Id) : IRequest<Result<CategoryDto>>;