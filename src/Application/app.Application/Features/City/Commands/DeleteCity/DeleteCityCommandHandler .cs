using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services;
using FluentValidation;
using MediatR;

namespace app.Application.Features.City.Commands.DeleteCity;

public class DeleteCityCommandHandler(ICityRepository cityRepository, ICacheService cache, IValidator<DeleteCityCommand> validator)
    : IRequestHandler<DeleteCityCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(DeleteCityCommand request, CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var result = await cityRepository.DeleteAsync(request.Id, ct);

        if (result)
        {
            var city = await cityRepository.GetByIdAsync(request.Id, ct);

            if (city != null)
            {
                await cache.RemoveAsync(CacheKeys.CityDetails(request.Id), ct);
                await cache.RemoveAsync(CacheKeys.CitiesByProvinceId(city.ProvinceId), ct);
            }
        }

        return !result
            ? Result<bool>.Failure("عملیات حذف با شکست مواجه شد")
            : Result<bool>.Success(result);
    }
    private static class CacheKeys
    {
        public static string CityDetails(int id) => $"city:details:{id}";
        public static string CitiesByProvinceId(int provinceId) => $"cities:province:{provinceId}";
    }
}
