using app.Application.Contracts.Repositories;
using MediatR;

namespace app.Application.Features.City.Queries.GetCitiesCount;

public class GetCitiesCountQueryHandler(ICityRepository cityRepository) : IRequestHandler<GetCitiesCountQuery, int>
{

    public async Task<int> Handle(GetCitiesCountQuery request, CancellationToken ct)
    {
        return await cityRepository.GetCountAsync(ct);
    }
}
