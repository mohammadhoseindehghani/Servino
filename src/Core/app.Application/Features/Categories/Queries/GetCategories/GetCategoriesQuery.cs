using app.Application.DTOs.CategoryDTOs;
using MediatR;

namespace app.Application.Features.Categories.Queries.GetCategories;

public record GetCategoriesQuery(int PageNumber, int PageSize, string? SearchKey) 
    : IRequest<List<CategorySummaryDto>>;