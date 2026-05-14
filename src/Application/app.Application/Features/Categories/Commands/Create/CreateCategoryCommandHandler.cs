using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Providers_Services;
using app.Application.Contracts.Contracts.Services.CategoryAgg;
using app.Domain.CategoryAgg.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Categories.Commands.Create;

public class CreateCategoryCommandHandler(
    ICategoryService categoryService,
    ICacheService cache,
    ILogger<CreateCategoryCommandHandler> logger,
    IValidator<CreateCategoryCommand> validator,
    IFileService fileService)
    : IRequestHandler<CreateCategoryCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateCategoryCommand request, CancellationToken ct)
    {
        var validation = await validator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return Result<int>.Failure(validation.Errors.First().ErrorMessage, "VALIDATION_ERROR");

        string? imagePath = null;

        try
        {
            if (request.Image != null)
                imagePath = await fileService.UploadAsync(request.Image, "Categories", ct);

            var category = new Category(request.Title, imagePath, request.ParentId);

            await categoryService.CreateAsync(category, ct);

            await cache.BumpStampAsync("stamp:requests:all", TimeSpan.FromHours(6), ct);
            await cache.BumpStampAsync("stamp:requests:available", TimeSpan.FromHours(6), ct);

            return Result<int>.Success(category.Id, "دسته‌بندی با موفقیت ایجاد شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating category");

            if (imagePath != null)
                await fileService.DeleteFileAsync(imagePath, ct);

            throw; // for global exception 
        }
    }
}



