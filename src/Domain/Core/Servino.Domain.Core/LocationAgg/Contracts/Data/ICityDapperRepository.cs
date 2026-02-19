using Servino.Domain.Core._common;
using Servino.Domain.Core.LocationAgg.Dtos;

namespace Servino.Domain.Core.LocationAgg.Contracts.Data;

public interface ICityDapperRepository
{
    Task<CityDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<List<CityDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct);
    Task<List<SelectListDto>> GetCitiesByProvinceIdAsync(int provinceId, CancellationToken ct);
}