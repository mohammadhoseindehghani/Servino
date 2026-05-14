using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.ProvinceAgg;
using MediatR;

namespace app.Application.Features.Province.Queries.GetProvincesCount;

public class GetProvincesCountQueryHandler(IProvinceService provinceService)
    : IRequestHandler<GetProvincesCountQuery, int>
{

    public async Task<int> Handle(GetProvincesCountQuery request, CancellationToken ct)
    {
        return await provinceService.GetCountAsync(ct);
    }
}
