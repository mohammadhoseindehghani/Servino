using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Providers_Services;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.HomeServiceAgg;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.HomeServices.Commands.DeleteHomeService;

public class DeleteHomeServiceCommandHandler(
    IHomeServiceService homeServiceService,
    ICacheService cache,
    ILogger<DeleteHomeServiceCommandHandler> logger,
    IValidator<DeleteHomeServiceCommand> validator)
    : IRequestHandler<DeleteHomeServiceCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(DeleteHomeServiceCommand request, CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        try
        {
            var isDeleted = await homeServiceService.DeleteAsync(request.Id, ct);

            if (isDeleted)
            {
                await cache.RemoveAsync(CacheKeys.HomeServiceDetails(request.Id), ct);
                await cache.RemoveAsync(CacheKeys.HomeServicesAll("", 1, 10), ct);
            }

            return !isDeleted
                ? Result<bool>.Failure("خدمت یافت نشد.")
                : Result<bool>.Success(true, "خدمت با موفقیت حذف شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error while deleting HomeService | Id: {Id}",
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
