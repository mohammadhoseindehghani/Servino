using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.ExpertHomeServiceAgg;
using app.Application.Contracts.Contracts.Services.UserAgg;
using app.Domain.ExpertHomeServiceAgg.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Experts.Commands.UpdateExpertServices;

public class UpdateExpertServicesCommandHandler(
    IExpertService expertService,
    IExpertHomeServiceService expertHomeServiceService,
    ILogger<UpdateExpertServicesCommandHandler> logger,
    IValidator<UpdateExpertServicesCommand> validator)
    : IRequestHandler<UpdateExpertServicesCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        UpdateExpertServicesCommand request,
        CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var expertDto = await expertService.GetByUserId(request.UserId, ct);
        if (expertDto == null)
            return Result<bool>.Failure("اکسپرت یافت نشد.");

        try
        {
            await expertHomeServiceService.UpdateExpertServicesAsync(expertDto.ExpertId, request.SelectedIds, ct);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error in UpdateExpertServicesCommandHandler | UserId: {UserId}",
                request.UserId);

            return Result<bool>.Failure("خطا در به‌روزرسانی سرویس‌ها");
        }
    }
}
