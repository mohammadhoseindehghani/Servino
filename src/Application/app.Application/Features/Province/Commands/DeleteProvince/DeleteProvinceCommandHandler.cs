using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Province.Commands.DeleteProvince;

public class DeleteProvinceCommandHandler(
    IProvinceRepository provinceRepository,
    ICacheService cache,
    ILogger<DeleteProvinceCommandHandler> logger,
    IValidator<DeleteProvinceCommand> validator)
    : IRequestHandler<DeleteProvinceCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(DeleteProvinceCommand request, CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        try
        {
            var result = await provinceRepository.DeleteAsync(request.Id, ct);

            if (result)
            {
                await cache.RemoveAsync(CacheKeys.ProvinceDetails(request.Id), ct);
                await cache.RemoveAsync(CacheKeys.ProvincesForDropdown, ct);
            }

            return !result
                ? Result<bool>.Failure("حذف با شکست مواجه شد")
                : Result<bool>.Success(true, "استان با موفقیت حذف شد");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting province. Id={Id}", request.Id);
            return Result<bool>.Failure("خطای سیستمی رخ داده است");
        }
    }
    private static class CacheKeys
    {
        public static string ProvinceDetails(int id) => $"province:details:{id}";
        public static string ProvincesForDropdown => "provinces:dropdown";
    }
}
