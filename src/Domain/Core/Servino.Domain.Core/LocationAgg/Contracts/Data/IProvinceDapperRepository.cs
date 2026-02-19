using Servino.Domain.Core._common;
using Servino.Domain.Core.LocationAgg.Dtos;

namespace Servino.Domain.Core.LocationAgg.Contracts.Data;

public interface IProvinceDapperRepository
{
    Task<ProvinceDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<List<ProvinceDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct);
    Task<List<SelectListDto>> GetAllForDropdownAsync(CancellationToken ct);
}