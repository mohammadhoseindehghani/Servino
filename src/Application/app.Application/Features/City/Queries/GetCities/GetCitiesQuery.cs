using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.LocationDTOs;
using MediatR;

namespace app.Application.Features.City.Queries.GetCities;

public record GetCitiesQuery(PaginationRequestDto Search) : IRequest<List<CityDto>>;
