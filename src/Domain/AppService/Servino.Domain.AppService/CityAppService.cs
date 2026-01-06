using Servino.Domain.Core._common;
using Servino.Domain.Core.LocationAgg.Contracts.AppService;
using Servino.Domain.Core.LocationAgg.Contracts.Service;
using Servino.Domain.Core.LocationAgg.Dtos;

namespace Servino.Domain.AppService;

public class CityAppService(ICityService cityService) : ICityAppService
{
    public async Task<Result<bool>> CreateAsync(string title, int provinceId, CancellationToken ct)
    {
        var result = await cityService.CreateAsync(title, provinceId, ct);
        return !result ? Result<bool>.Failure("عملیات ایجاد با شکست مواجه شد") : Result<bool>.Success(result);
    }

    public async Task<Result<bool>> UpdateAsync(int id, string title, int provinceId, CancellationToken ct)
    {
        var result = await cityService.UpdateAsync(id, title, provinceId, ct);
        return !result ? Result<bool>.Failure("عملیات اپدیت با شکست مواجه شد") : Result<bool>.Success(result);
    }

    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken ct)
    {
        var result = await cityService.DeleteAsync(id, ct);
        return !result ? Result<bool>.Failure("عملیات حذف با شکست مواجه شد") : Result<bool>.Success(result);
    }

    public async Task<Result<CityDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var result = await cityService.GetByIdAsync(id, ct);
        return result is null ? Result<CityDto>.Failure("شهری یافت نشد ") : Result<CityDto>.Success(result);
    }

    public async Task<List<CityDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        return await cityService.GetAllAsync(search, ct);
    }

    public async Task<int> GetCountAsync(CancellationToken ct)
    {
        return await cityService.GetCountAsync(ct);
    }

    public async Task<List<SelectListDto>> GetCitiesByProvinceIdAsync(int provinceId, CancellationToken ct)
    {
        return await cityService.GetCitiesByProvinceIdAsync(provinceId, ct);
    }
}