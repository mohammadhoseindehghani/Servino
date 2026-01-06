using Servino.Domain.Core._common;
using Servino.Domain.Core.LocationAgg.Dtos;

namespace Servino.Domain.Core.LocationAgg.Contracts.AppService;

public interface ICityAppService
{
    Task<Result<bool>> CreateAsync(string title, int provinceId, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, string title, int provinceId, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, CancellationToken ct);
    Task<Result<CityDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<List<CityDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct);
    Task<int> GetCountAsync(CancellationToken ct);

    Task<List<SelectListDto>> GetCitiesByProvinceIdAsync(int provinceId, CancellationToken ct);
}