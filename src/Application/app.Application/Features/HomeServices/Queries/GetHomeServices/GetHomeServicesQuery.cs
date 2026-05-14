using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.HomeServiceDTOs;
using MediatR;

namespace app.Application.Features.HomeServices.Queries.GetHomeServices;

public record GetHomeServicesQuery(PaginationRequestDto Search)
    : IRequest<Result<List<HomeServiceSummaryDto>>>;
