using Servino.Domain.Core._common;
using Servino.Domain.Core.LocationAgg.Dtos;

namespace Servino.Domain.Core.LocationAgg.Contracts.AppService;

public interface IProvinceAppService
{
    Task<Result<bool>> CreateAsync(string title, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, string title, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, CancellationToken ct); 
    Task<Result<ProvinceDto>> GetByIdAsync(int id, CancellationToken ct);

    Task<List<ProvinceDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct);
    Task<int> GetCountAsync(CancellationToken ct);

    Task<List<SelectListDto>> GetAllForDropdownAsync(CancellationToken ct);
}