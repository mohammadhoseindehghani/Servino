using app.Application.Contracts.DTOs.CategoryDTOs;
using MediatR;

namespace app.Application.Features.Categories.Queries.GetBreadcrumb;

public record GetCategoryBreadcrumbQuery(int CategoryId) : IRequest<List<BreadcrumbDto>>;