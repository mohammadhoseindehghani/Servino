using app.Application.Common;
using app.Application.DTOs.LocationDTOs;
using MediatR;

namespace app.Application.Features.City.Queries.GetCities;

public record GetCitiesQuery(PaginationRequestDto Search) : IRequest<List<CityDto>>;
