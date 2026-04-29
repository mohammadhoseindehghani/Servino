using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.Contracts.Services;
using MediatR;

namespace app.Application.Features.City.Commands.CreateCity;

public class CreateCityCommandHandler(ICityRepository cityRepository, ICacheService cache)
    : IRequestHandler<CreateCityCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateCityCommand request, CancellationToken ct)
    {
        var result = await cityRepository.CreateAsync(request.Title, request.ProvinceId, ct);

        if (result)
        {
            await cache.RemoveAsync(CacheKeys.CitiesAll("", 1, 10), ct);
            await cache.RemoveAsync(CacheKeys.CitiesByProvinceId(request.ProvinceId), ct);
        }

        return !result
            ? Result<bool>.Failure("عملیات ایجاد با شکست مواجه شد")
            : Result<bool>.Success(result);
    }
    private static class CacheKeys
    { 
        public static string CitiesAll(string searchKey, int pageNumber, int pageSize) =>
            $"cities:all:{searchKey}:{pageNumber}:{pageSize}";
        public static string CitiesByProvinceId(int provinceId) => $"cities:province:{provinceId}";
    }
}
