using app.Application.Common;
using app.Application.Contracts.Persistence;
using app.Application.Contracts.Services;
using app.Domain.CategoryAgg.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Categories.Commands.Create;

public class CreateCategoryHandler(
    ICategoryRepository categoryRepository,
    ICacheService cache,
    ILogger<CreateCategoryHandler> logger)
    : IRequestHandler<CreateCategoryCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateCategoryCommand request, CancellationToken ct)
    {
        try
        {
            var category = new Category
            {
                Title = request.Title,
                ParentId = request.ParentId
            };

            await categoryRepository.CreateAsync(category, ct);

            await cache.BumpStampAsync("stamp:requests:all", TimeSpan.FromHours(6), ct);
            await cache.BumpStampAsync("stamp:requests:available", TimeSpan.FromHours(6), ct);

            return Result<bool>.Success(true, "دسته‌بندی با موفقیت ایجاد شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "System error in CreateCategoryHandler | Title: {Title} | ParentId: {ParentId}",
                request.Title, request.ParentId);

            return Result<bool>.Failure("خطای سیستمی رخ داده است.");
        }
    }
}

