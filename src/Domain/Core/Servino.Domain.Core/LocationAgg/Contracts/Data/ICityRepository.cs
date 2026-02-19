using Servino.Domain.Core._common;
using Servino.Domain.Core.LocationAgg.Dtos;

namespace Servino.Domain.Core.LocationAgg.Contracts.Data;

public interface ICityRepository
{
    Task<bool> CreateAsync(string title, int provinceId, CancellationToken ct);
    Task<bool> UpdateAsync(int id, string title, int provinceId, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<CityDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<List<CityDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct);
    Task<int> GetCountAsync(CancellationToken ct);

    Task<List<SelectListDto>> GetCitiesByProvinceIdAsync(int provinceId, CancellationToken ct);
}