using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.Contracts.Services;
using app.Application.DTOs.HomeServiceDTOs;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.HomeServices.Commands.CreateHomeService;

public class CreateHomeServiceCommandHandler(
    IHomeServiceRepository homeServiceRepository,
    ICacheService cache,
    ILogger<CreateHomeServiceCommandHandler> logger,
    IValidator<CreateHomeServiceCommand> validator)
    : IRequestHandler<CreateHomeServiceCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(CreateHomeServiceCommand request, CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        try
        {
            var dto = new HomeServiceDto
            {
                Title = request.Title,
                BasePrice = request.BasePrice,
                CategoryId = request.CategoryId
            };

            var isCreated = await homeServiceRepository.CreateAsync(dto, ct);

            if (isCreated)
                await cache.RemoveAsync(CacheKeys.HomeServicesAll(request.Title, 1, 10), ct);

            return !isCreated
                ? Result<bool>.Failure("خطایی در ثبت خدمت رخ داد.")
                : Result<bool>.Success(true, "خدمت جدید با موفقیت ثبت شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error while creating HomeService | CategoryId: {CategoryId} | Title: {Title}",
                request.CategoryId,
                request.Title);

            return Result<bool>.Failure("خطای سیستمی رخ داده است.");
        }
    }
    private static class CacheKeys
    { 
        public static string HomeServicesAll(string searchKey, int pageNumber, int pageSize) =>
            $"homeServices:all:{searchKey}:{pageNumber}:{pageSize}";
    }
}
