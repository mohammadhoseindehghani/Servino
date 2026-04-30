using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Domain.ExpertHomeServiceAgg.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Experts.Commands.UpdateExpertServices;

public class UpdateExpertServicesCommandHandler(
    IExpertRepository expertRepository,
    IExpertHomeServiceRepository expertHomeServiceRepository,
    ILogger<UpdateExpertServicesCommandHandler> logger)
    : IRequestHandler<UpdateExpertServicesCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        UpdateExpertServicesCommand request,
        CancellationToken ct)
    {
        var expertId = await expertRepository.GetIdByUserIdAsync(request.UserId, ct);
        if (expertId == 0)
            return Result<bool>.Failure("اکسپرت یافت نشد.");

        try
        {
            await expertHomeServiceRepository.DeleteAllByExpertIdAsync(expertId, ct);

            if (request.SelectedIds != null! && request.SelectedIds.Any())
            {
                var list = request.SelectedIds.Select(id => new ExpertHomeService
                {
                    ExpertId = expertId,
                    HomeServiceId = id
                }).ToList();

                await expertHomeServiceRepository.AddRangeAsync(list, ct);
            }
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
