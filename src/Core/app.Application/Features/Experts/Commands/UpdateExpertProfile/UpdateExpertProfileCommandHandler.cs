using app.Application.Common;
using app.Application.Contracts.Repositories;
using MediatR;

namespace app.Application.Features.Experts.Commands.UpdateExpertProfile;

public class UpdateExpertProfileCommandHandler(IExpertRepository expertRepository)
    : IRequestHandler<UpdateExpertProfileCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        UpdateExpertProfileCommand request,
        CancellationToken ct)
    {
        var result = await expertRepository.UpdateProfile(request.Profile, ct);

        return !result
            ? Result<bool>.Failure("عملیات آپدیت با شکست مواجه شد")
            : Result<bool>.Success(true, "آپدیت اطلاعات با موفقیت انجام شد");
    }
}
