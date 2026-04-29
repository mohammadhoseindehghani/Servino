using app.Application.Common;
using app.Application.DTOs.LocationDTOs;
using MediatR;

namespace app.Application.Features.Province.Queries.GetProvinces;

public record GetProvincesQuery(PaginationRequestDto Search)
    : IRequest<List<ProvinceDto>>;
