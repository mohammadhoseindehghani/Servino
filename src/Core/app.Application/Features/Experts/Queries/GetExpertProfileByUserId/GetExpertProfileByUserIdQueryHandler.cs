using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.DTOs.UserDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Experts.Queries.GetExpertProfileByUserId;

public class GetExpertProfileByUserIdQueryHandler(
    IExpertRepository expertRepository,
    ILogger<GetExpertProfileByUserIdQueryHandler> logger)
    : IRequestHandler<GetExpertProfileByUserIdQuery, Result<ExpertProfileDto>>
{
    public async Task<Result<ExpertProfileDto>> Handle(
        GetExpertProfileByUserIdQuery request,
        CancellationToken ct)
    {
        try
        {
            var profile = await expertRepository.GetByUserIdAsync(request.UserId, ct);

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
