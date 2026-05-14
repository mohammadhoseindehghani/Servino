using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.LocationDTOs;
using MediatR;

namespace app.Application.Features.City.Queries.GetCityById;

public record GetCityByIdQuery(int Id) : IRequest<Result<CityDto>>;
