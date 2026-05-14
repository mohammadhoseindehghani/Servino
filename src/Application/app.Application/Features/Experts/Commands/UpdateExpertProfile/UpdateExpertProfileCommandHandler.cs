using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Services.UserAgg;
using FluentValidation;
using MediatR;

namespace app.Application.Features.Experts.Commands.UpdateExpertProfile;

public class UpdateExpertProfileCommandHandler(IExpertService expertService, IValidator<UpdateExpertProfileCommand> validator)
    : IRequestHandler<UpdateExpertProfileCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        UpdateExpertProfileCommand request,
        CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var result = await expertService.UpdateProfile(request.Profile, ct);

        return !result
            ? Result<bool>.Failure("عملیات آپدیت با شکست مواجه شد")
            : Result<bool>.Success(true, "آپدیت اطلاعات با موفقیت انجام شد");
    }
}
