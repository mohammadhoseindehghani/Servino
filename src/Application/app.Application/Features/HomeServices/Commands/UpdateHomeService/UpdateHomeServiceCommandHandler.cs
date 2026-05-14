using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Providers_Services;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.DTOs.HomeServiceDTOs;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.HomeServices.Commands.UpdateHomeService;

public class UpdateHomeServiceCommandHandler(
    IHomeServiceRepository homeServiceRepository,
    ICacheService cache,
    ILogger<UpdateHomeServiceCommandHandler> logger,
    IValidator<UpdateHomeServiceCommand> validator)
    : IRequestHandler<UpdateHomeServiceCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(UpdateHomeServiceCommand request, CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        try
        {
            var dto = new HomeServiceDto
            {
                Id = request.Id,
                Title = request.Title,
                BasePrice = request.BasePrice,
                CategoryId = request.CategoryId
            };

            var isUpdated = await homeServiceRepository.UpdateAsync(dto, ct);

            if (isUpdated)
            {
                await cache.RemoveAsync(CacheKeys.HomeServiceDetails(request.Id), ct);
                await cache.RemoveAsync(CacheKeys.HomeServicesAll(request.Title, 1, 10), ct);
            }

            return !isUpdated
                ? Result<bool>.Failure("خدمت یافت نشد یا ویرایش انجام نشد.")
                : Result<bool>.Success(true, "خدمت با موفقیت ویرایش شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error while updating HomeService | Id: {Id}",
                request.Id);

            return Result<bool>.Failure("خطای سیستمی رخ داده است.");
        }
    }
    private static class CacheKeys
    {
        public static string HomeServiceDetails(int id) => $"homeService:details:{id}";
        public static string HomeServicesAll(string searchKey, int pageNumber, int pageSize) =>
            $"homeServices:all:{searchKey}:{pageNumber}:{pageSize}";
    }
}
