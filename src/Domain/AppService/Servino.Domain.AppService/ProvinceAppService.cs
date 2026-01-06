using Servino.Domain.Core._common;
using Servino.Domain.Core.LocationAgg.Contracts.AppService;
using Servino.Domain.Core.LocationAgg.Contracts.Service;
using Servino.Domain.Core.LocationAgg.Dtos;

namespace Servino.Domain.AppService;

public class ProvinceAppService(IProvinceService provinceService) : IProvinceAppService
{
    public async Task<Result<bool>> CreateAsync(string title, CancellationToken ct)
    {
        var result = await provinceService.CreateAsync(title, ct);
        return !result ? Result<bool>.Failure("استان اینجاد نشد") : Result<bool>.Success(result);
    }

    public async Task<Result<bool>> UpdateAsync(int id, string title, CancellationToken ct)
    {
        var result = await provinceService.UpdateAsync(id, title, ct);
        return !result ? Result<bool>.Failure("اپدیت استان با شکست مواجه شد.") : Result<bool>.Success(result);
    }

    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken ct)
    {
        var result = await provinceService.DeleteAsync(id, ct);
        return !result ? Result<bool>.Failure("حذف با شکست مواجه شد") : Result<bool>.Success(result);
    }

    public async Task<Result<ProvinceDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var result = await provinceService.GetByIdAsync(id, ct);
        return result is null ? Result<ProvinceDto>.Failure("استانی یافت نشد") : Result<ProvinceDto>.Success(result);
    }

    public async Task<List<ProvinceDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        return await provinceService.GetAllAsync(search, ct);
    }

    public async Task<int> GetCountAsync(CancellationToken ct)
    {
        return await provinceService.GetCountAsync(ct);
    }

    public async Task<List<SelectListDto>> GetAllForDropdownAsync(CancellationToken ct)
    {
        return await provinceService.GetAllForDropdownAsync(ct);
    }
}