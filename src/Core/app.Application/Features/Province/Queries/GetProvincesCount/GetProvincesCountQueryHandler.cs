using app.Application.Contracts.Repositories;
using MediatR;

namespace app.Application.Features.Province.Queries.GetProvincesCount;

public class GetProvincesCountQueryHandler(IProvinceRepository provinceRepository)
    : IRequestHandler<GetProvincesCountQuery, int>
{

    public async Task<int> Handle(GetProvincesCountQuery request, CancellationToken ct)
    {
        return await provinceRepository.GetCountAsync(ct);
    }
}
