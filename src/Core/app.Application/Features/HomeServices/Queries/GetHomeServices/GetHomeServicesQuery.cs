using app.Application.Common;
using app.Application.DTOs.HomeServiceDTOs;
using MediatR;

namespace app.Application.Features.HomeServices.Queries.GetHomeServices;

public record GetHomeServicesQuery(PaginationRequestDto Search)
    : IRequest<Result<List<HomeServiceSummaryDto>>>;
