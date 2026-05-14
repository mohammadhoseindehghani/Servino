using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.ProvinceAgg;
using app.Application.Contracts.DTOs.LocationDTOs;

namespace app.Application.Features.Province.Services;

public class ProvinceService(IProvinceRepository provinceRepo) : IProvinceService
{
    public async Task<bool> CreateAsync(string title, CancellationToken ct)
    {
        return await provinceRepo.CreateAsync(title, ct);
    }

    public async Task<bool> UpdateAsync(int id, string title, CancellationToken ct)
    {
        return await provinceRepo.UpdateAsync(id, title, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        return await provinceRepo.DeleteAsync(id, ct);
    }

    public async Task<ProvinceDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await provinceRepo.GetByIdAsync(id, ct);
    }

    public async Task<List<ProvinceDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        return await provinceRepo.GetAllAsync(search, ct);
    }

    public async Task<int> GetCountAsync(CancellationToken ct)
    {
        return await provinceRepo.GetCountAsync(ct);
    }

    public async Task<List<SelectListDto>> GetAllForDropdownAsync(CancellationToken ct)
    {
        return await provinceRepo.GetAllForDropdownAsync(ct);
    }
}