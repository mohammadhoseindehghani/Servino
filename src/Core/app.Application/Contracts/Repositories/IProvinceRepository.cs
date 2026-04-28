using app.Application.Common;
using app.Application.DTOs.LocationDTOs;

namespace app.Application.Contracts.Repositories;

public interface IProvinceRepository
{
    Task<bool> CreateAsync(string title, CancellationToken ct);
    Task<bool> UpdateAsync(int id, string title, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct); 
    Task<ProvinceDto?> GetByIdAsync(int id, CancellationToken ct);

    Task<List<ProvinceDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct);
    Task<int> GetCountAsync(CancellationToken ct); 

    Task<List<SelectListDto>> GetAllForDropdownAsync(CancellationToken ct);
}