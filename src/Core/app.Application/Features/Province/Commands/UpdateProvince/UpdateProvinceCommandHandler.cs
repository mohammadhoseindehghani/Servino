using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.Contracts.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Province.Commands.UpdateProvince;

public class UpdateProvinceCommandHandler(
    IProvinceRepository provinceRepository,
    ICacheService cache,
    ILogger<UpdateProvinceCommandHandler> logger)
    : IRequestHandler<UpdateProvinceCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(UpdateProvinceCommand request, CancellationToken ct)
    {
        try
        {
            var result = await provinceRepository.UpdateAsync(request.Id, request.Title, ct);

            if (result)
            {
                await cache.RemoveAsync(CacheKeys.ProvinceDetails(request.Id), ct);
                await cache.RemoveAsync(CacheKeys.ProvincesForDropdown, ct);
            }

            return !result
                ? Result<bool>.Failure("آپدیت استان با شکست مواجه شد.")
                : Result<bool>.Success(true, "استان با موفقیت ویرایش شد");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating province. Id={Id}", request.Id);
            return Result<bool>.Failure("خطای سیستمی رخ داده است");
        }
    }
    private static class CacheKeys
    {
        public static string ProvinceDetails(int id) => $"province:details:{id}";
        public static string ProvincesForDropdown => "provinces:dropdown";
    }
}
