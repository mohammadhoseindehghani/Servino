using app.Application.Contracts.DTOs.CategoryDTOs;
using MediatR;

namespace app.Application.Features.Categories.Queries.GetCategoriesByParentId;

public record GetCategoriesByParentIdQuery(int? ParentId) : IRequest<List<CategoryClientDto>>;