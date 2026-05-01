using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.Contracts.Services;
using app.Application.DTOs.CategoryDTOs;
using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace app.Application.Features.Categories.Commands.Create;

public class CreateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    ICacheService cache,
    ILogger<CreateCategoryCommandHandler> logger,
    IValidator<CreateCategoryCommand> validator)
    : IRequestHandler<CreateCategoryCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateCategoryCommand request, CancellationToken ct)
    {
        var result = await validator.ValidateAsync(request, ct);

        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        try
        {
            var dto = new CategoryDto
            {
                Title = request.Title,
                ImagePath = request.ImagePath,
                ParentId = request.ParentId
            };
            var isCreated = await categoryRepository.CreateAsync(dto, ct);

            await cache.BumpStampAsync(CacheKeys.StampAllRequests, CacheTtl.Stamps, ct);
            await cache.BumpStampAsync(CacheKeys.StampAvailableRequests, CacheTtl.Stamps, ct);


            return !isCreated
                ? Result<bool>.Failure("خطایی در ایجاد دسته‌بندی رخ داد. ممکن است عنوان تکراری باشد.")
                : Result<bool>.Success(true, "دسته‌بندی با موفقیت ایجاد شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "System error in CategoryAppService.CreateAsync | Title: {Title} | ParentId: {ParentId}",
                request.Title, request.ParentId);
            return Result<bool>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }


    private static class CacheKeys
    {
        public static string StampAllRequests => "stamp:requests:all";
        public static string StampAvailableRequests => "stamp:requests:available";
    }

    private static class CacheTtl
    {
        public static readonly TimeSpan Stamps = TimeSpan.FromHours(6);
    }
}



