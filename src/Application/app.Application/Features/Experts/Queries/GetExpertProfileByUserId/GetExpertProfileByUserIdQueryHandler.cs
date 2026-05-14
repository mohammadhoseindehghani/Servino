using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.UserAgg;
using app.Application.Contracts.DTOs.UserDTOs;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Experts.Queries.GetExpertProfileByUserId;

public class GetExpertProfileByUserIdQueryHandler(
    IExpertService expertService,
    ILogger<GetExpertProfileByUserIdQueryHandler> logger,
    IValidator<GetExpertProfileByUserIdQuery> validator)
    : IRequestHandler<GetExpertProfileByUserIdQuery, Result<ExpertProfileDto>>
{
    public async Task<Result<ExpertProfileDto>> Handle(
        GetExpertProfileByUserIdQuery request,
        CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        try
        {
            var profile = await expertService.GetByUserId(request.UserId, ct);

            return profile == null
                ? Result<ExpertProfileDto>.Failure("پروفایلی یافت نشد")
                : Result<ExpertProfileDto>.Success(profile);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "System error in GetExpertProfileByUserIdQueryHandler | UserId: {UserId}",
                request.UserId);

            return Result<ExpertProfileDto>.Failure(
                "خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }
}
