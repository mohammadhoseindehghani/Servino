using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.Contracts.Services;
using MediatR;

namespace app.Application.Features.City.Commands.UpdateCity;

public class UpdateCityCommandHandler(ICityRepository cityRepository, ICacheService cache)
    : IRequestHandler<UpdateCityCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(UpdateCityCommand request, CancellationToken ct)
    {
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
