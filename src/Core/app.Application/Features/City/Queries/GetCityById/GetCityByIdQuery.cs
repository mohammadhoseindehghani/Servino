using app.Application.Common;
using app.Application.DTOs.LocationDTOs;
using MediatR;

namespace app.Application.Features.City.Queries.GetCityById;

public record GetCityByIdQuery(int Id) : IRequest<Result<CityDto>>;
