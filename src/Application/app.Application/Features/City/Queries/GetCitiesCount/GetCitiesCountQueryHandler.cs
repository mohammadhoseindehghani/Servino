using app.Application.Contracts.Contracts.Services.CityAgg;
using MediatR;

namespace app.Application.Features.City.Queries.GetCitiesCount;

public class GetCitiesCountQueryHandler(ICityService cityService) : IRequestHandler<GetCitiesCountQuery, int>
{
    public async Task<int> Handle(GetCitiesCountQuery request, CancellationToken ct)
    {
        return await cityService.GetCountAsync(ct);
    }
}
