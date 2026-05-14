using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.LocationDTOs;

namespace app.Application.Contracts.Contracts.Repositories;

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