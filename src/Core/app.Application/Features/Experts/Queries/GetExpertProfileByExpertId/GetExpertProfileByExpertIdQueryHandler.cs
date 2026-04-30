using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Experts.Queries.GetExpertProfileByExpertId;

public class GetExpertProfileByExpertIdQueryHandler(IExpertRepository expertRepository)
    : IRequestHandler<GetExpertProfileByExpertIdQuery, Result<ExpertProfileDto>>
{
    public async Task<Result<ExpertProfileDto>> Handle(
        GetExpertProfileByExpertIdQuery request,
        CancellationToken ct)
    {
        var profile = await expertRepository.GetByExpertIdAsync(request.ExpertId, ct);

        return profile == null
            ? Result<ExpertProfileDto>.Failure("مشخصات متخصص یافت نشد")
            : Result<ExpertProfileDto>.Success(profile);
    }
}
