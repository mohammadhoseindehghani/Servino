using app.Application.DTOs.CategoryDTOs;
using MediatR;

namespace app.Application.Features.Categories.Queries.GetServicesByCategoryId;

public record GetServicesByCategoryIdQuery(int CategoryId) : IRequest<List<ServiceClientDto>>;