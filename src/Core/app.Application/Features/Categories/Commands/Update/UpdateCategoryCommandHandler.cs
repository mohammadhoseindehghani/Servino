using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.Contracts.Services;
using app.Application.DTOs.CategoryDTOs;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Categories.Commands.Update;

public class UpdateCategoryCommandHandler (
    ICategoryRepository categoryRepository,
    ICacheService cache,
    ILogger<UpdateCategoryCommandHandler> logger,
    IValidator<UpdateCategoryCommand> validator) : IRequestHandler<UpdateCategoryCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateCategoryCommand request, CancellationToken ct)
    {
        var result = await validator.ValidateAsync(request, ct);

        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        var dto = new CategoryDto
        {
            Title = request.Title,
            ImagePath = request.ImagePath,
            ParentId = request.ParentId
        };
        try
        {
            var isUpdated = await categoryRepository.UpdateAsync(dto, ct);

            if (isUpdated)
            {
                await cache.RemoveAsync(CacheKeys.CategoryDetails(dto.Id), ct);
                await cache.BumpStampAsync(CacheKeys.StampAllRequests, CacheTtl.Stamps, ct);
                await cache.BumpStampAsync(CacheKeys.StampAvailableRequests, CacheTtl.Stamps, ct);
            }

            return !isUpdated ? Result<bool>.Failure("دسته‌بندی یافت نشد یا ویرایش انجام نشد.")
                : Result<bool>.Success(true, "دسته‌بندی با موفقیت ویرایش شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "System error in CategoryAppService.UpdateAsync | CategoryId: {CategoryId} | Title: {Title}",
                request.Id, request.Title);
            return Result<bool>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }

    private static class CacheKeys
    {
        public static string CategoryDetails(int id) => $"category:details:{id}";
        public static string StampAllRequests => "stamp:requests:all";
        public static string StampAvailableRequests => "stamp:requests:available";
    }
    private static class CacheTtl
    {
        public static readonly TimeSpan Stamps = TimeSpan.FromHours(6);
    }
}