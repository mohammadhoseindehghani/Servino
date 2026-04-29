using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.Contracts.Services;
using MediatR;

namespace app.Application.Features.City.Commands.DeleteCity;

public class DeleteCityCommandHandler(ICityRepository cityRepository, ICacheService cache)
    : IRequestHandler<DeleteCityCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(DeleteCityCommand request, CancellationToken ct)
    {
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
