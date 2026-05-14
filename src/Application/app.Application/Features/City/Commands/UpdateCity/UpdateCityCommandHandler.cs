using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Providers_Services;
using app.Application.Contracts.Contracts.Repositories;
using FluentValidation;
using MediatR;

namespace app.Application.Features.City.Commands.UpdateCity;

public class UpdateCityCommandHandler(ICityRepository cityRepository, ICacheService cache, IValidator<UpdateCityCommand> validator)
    : IRequestHandler<UpdateCityCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateCityCommand request, CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var result = await cityRepository.UpdateAsync(request.Id, request.Title, request.ProvinceId, ct);

        if (result)
        {
            await cache.RemoveAsync(CacheKeys.CityDetails(request.Id), ct);
            await cache.RemoveAsync(CacheKeys.CitiesByProvinceId(request.ProvinceId), ct);
        }

        return !result
            ? Result<bool>.Failure("عملیات اپدیت با شکست مواجه شد")
            : Result<bool>.Success(result);
    }
    private static class CacheKeys
    {
        public static string CityDetails(int id) => $"city:details:{id}";
        public static string CitiesByProvinceId(int provinceId) => $"cities:province:{provinceId}";
    }
}
