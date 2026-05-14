using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.DTOs.UserDTOs;
using FluentValidation;
using MediatR;

namespace app.Application.Features.Experts.Queries.GetExpertProfileByExpertId;

public class GetExpertProfileByExpertIdQueryHandler(IExpertRepository expertRepository, IValidator<GetExpertProfileByExpertIdQuery> validator)
    : IRequestHandler<GetExpertProfileByExpertIdQuery, Result<ExpertProfileDto>>
{
    public async Task<Result<ExpertProfileDto>> Handle(
        GetExpertProfileByExpertIdQuery request,
        CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var profile = await expertRepository.GetByExpertIdAsync(request.ExpertId, ct);

        return profile == null
            ? Result<ExpertProfileDto>.Failure("مشخصات متخصص یافت نشد")
            : Result<ExpertProfileDto>.Success(profile);
    }
}
