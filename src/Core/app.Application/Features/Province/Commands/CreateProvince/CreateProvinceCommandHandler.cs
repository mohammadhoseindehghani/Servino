using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.Contracts.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Province.Commands.CreateProvince;

public class CreateProvinceCommandHandler(
    IProvinceRepository provinceRepository,
    ICacheService cache,
    ILogger<CreateProvinceCommandHandler> logger)
    : IRequestHandler<CreateProvinceCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(CreateProvinceCommand request, CancellationToken ct)
    {
        try
        {
            var result = await provinceRepository.CreateAsync(request.Title, ct);

            if (result)
            {
                await cache.RemoveAsync(CacheKeys.ProvincesAll("", 1, 10), ct);
                await cache.RemoveAsync(CacheKeys.ProvincesForDropdown, ct);
            }

            return !result
                ? Result<bool>.Failure("استان ایجاد نشد")
                : Result<bool>.Success(true, "استان با موفقیت ایجاد شد");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating province. Title={Title}", request.Title);
            return Result<bool>.Failure("خطای سیستمی رخ داده است");
        }
    }
    private static class CacheKeys
    {
        public static string ProvincesAll(string searchKey, int pageNumber, int pageSize) =>
            $"provinces:all:{searchKey}:{pageNumber}:{pageSize}";
        public static string ProvincesForDropdown => "provinces:dropdown";
    }
}
