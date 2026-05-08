using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.Contracts.Services;
using app.Application.DTOs.CategoryDTOs;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Categories.Commands.Update;

public class UpdateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    ICacheService cache,
    ILogger<UpdateCategoryCommandHandler> logger,
    IValidator<UpdateCategoryCommand> validator,
    IFileService fileService) : IRequestHandler<UpdateCategoryCommand, Result>
{
    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken ct)
    {
        var result = await validator.ValidateAsync(request, ct);

        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        var category = await categoryRepository.GetByIdAsync(request.Id, ct);
        if (category == null)
            return Result.Failure("دسته بندی یافت نشد.", "NOT_FOUND");

        string? imagePath = null;

        if (request.Image != null)
        {
            await fileService.DeleteFileAsync(category.ImagePath, ct);
            imagePath = await fileService.UploadAsync(request.Image, "Categories", ct);
        }

        var dto = new CategoryDto
        {
            Title = request.Title,
            ImagePath = imagePath,
            ParentId = request.ParentId
        };

        try
        {
            await categoryRepository.UpdateAsync(dto, ct);

            await cache.RemoveAsync(CacheKeys.CategoryDetails(dto.Id), ct);
            await cache.BumpStampAsync(CacheKeys.StampAllRequests, CacheTtl.Stamps, ct);
            await cache.BumpStampAsync(CacheKeys.StampAvailableRequests, CacheTtl.Stamps, ct);

            return Result<bool>.Failure("دسته‌بندی یافت نشد یا ویرایش انجام نشد.");
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