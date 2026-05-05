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
    : IRequestHandler<CreateCategoryCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateCategoryCommand request, CancellationToken ct)
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
        var id = await categoryRepository.CreateAsync(dto, ct);

        await cache.BumpStampAsync(CacheKeys.StampAllRequests, CacheTtl.Stamps, ct);
        await cache.BumpStampAsync(CacheKeys.StampAvailableRequests, CacheTtl.Stamps, ct);

        if (id <= 0)
            Result<int>.Failure("خطایی در ایجاد دسته‌بندی رخ داد. ممکن است عنوان تکراری باشد.");
        
        return Result<int>.Success(id, "دسته‌بندی با موفقیت ایجاد شد.");

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



