using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.LocationDTOs;
using MediatR;

namespace app.Application.Features.Province.Queries.GetProvinces;

public record GetProvincesQuery(PaginationRequestDto Search)
    : IRequest<List<ProvinceDto>>;
