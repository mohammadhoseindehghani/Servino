using app.Application.Contracts.DTOs.LocationDTOs;
using MediatR;

namespace app.Application.Features.City.Queries.GetCitiesByProvince;

public record GetCitiesByProvinceQuery(int ProvinceId) : IRequest<List<SelectListDto>>;
