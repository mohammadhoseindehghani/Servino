using Servino.Domain.Core._common;
using Servino.Domain.Core.LocationAgg.Contracts.Data;
using Servino.Domain.Core.LocationAgg.Contracts.Service;
using Servino.Domain.Core.LocationAgg.Dtos;

namespace Servino.Domain.Service;

public class CityService(ICityRepository cityRepo) : ICityService
{
    public async Task<bool> CreateAsync(string title, int provinceId, CancellationToken ct)
    {
        return await cityRepo.CreateAsync(title, provinceId, ct);
    }

    public async Task<bool> UpdateAsync(int id, string title, int provinceId, CancellationToken ct)
    {
        return await cityRepo.UpdateAsync(id, title, provinceId, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        return await cityRepo.DeleteAsync(id, ct);
    }

    public async Task<CityDto?> GetByIdAsync(int id, CancellationToken ct)
    {
       return await cityRepo.GetByIdAsync(id, ct);
    }

    public async Task<List<CityDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        return await cityRepo.GetAllAsync(search, ct);
    }

    public async Task<int> GetCountAsync(CancellationToken ct)
    {
        return await cityRepo.GetCountAsync(ct);
    }

    public async Task<List<SelectListDto>> GetCitiesByProvinceIdAsync(int provinceId, CancellationToken ct)
    {
        return await cityRepo.GetCitiesByProvinceIdAsync(provinceId, ct);
    }
}