using MediatR;

namespace app.Application.Features.City.Queries.GetCitiesCount;

public record GetCitiesCountQuery() : IRequest<int>;
