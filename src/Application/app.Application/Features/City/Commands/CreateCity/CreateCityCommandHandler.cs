using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Providers_Services;
using app.Application.Contracts.Contracts.Repositories;
using FluentValidation;
using MediatR;

namespace app.Application.Features.City.Commands.CreateCity;

public class CreateCityCommandHandler(ICityRepository cityRepository, ICacheService cache, IValidator<CreateCityCommand> validator)
    : IRequestHandler<CreateCityCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateCityCommand request, CancellationToken ct)
    {
        var result = await validator.ValidateAsync(request, ct);

        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        var resultOp = await cityRepository.CreateAsync(request.Title, request.ProvinceId, ct);

        if (resultOp)
        {
            await cache.RemoveAsync(CacheKeys.CitiesAll("", 1, 10), ct);
            await cache.RemoveAsync(CacheKeys.CitiesByProvinceId(request.ProvinceId), ct);
        }

        return !resultOp
            ? Result<bool>.Failure("عملیات ایجاد با شکست مواجه شد")
            : Result<bool>.Success(resultOp);
    }
    private static class CacheKeys
    { 
        public static string CitiesAll(string searchKey, int pageNumber, int pageSize) =>
            $"cities:all:{searchKey}:{pageNumber}:{pageSize}";
        public static string CitiesByProvinceId(int provinceId) => $"cities:province:{provinceId}";
    }
}
