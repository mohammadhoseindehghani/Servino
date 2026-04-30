using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.DTOs.RequestDTOs;
using app.Domain.SuggestionAgg.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Requests.Commands.CancelRequest;

public class CancelRequestCommandHandler(
    IRequestRepository requestRepository,
    ILogger<CancelRequestCommandHandler> logger)
    : IRequestHandler<CancelRequestCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CancelRequestCommand command, CancellationToken ct)
    {
        try
        {
            var request = await requestRepository.GetByIdAsync(command.RequestId, ct);

            if (request == null)
                return Result<bool>.Failure("درخواست یافت نشد.");

            if (request.CustomerId != command.CustomerId)
                return Result<bool>.Failure("شما اجازه لغو این درخواست را ندارید.");

            if (request.Status != RequestStatus.WaitingForExperts &&
                request.Status != RequestStatus.WaitingForSelection)
            {
                return Result<bool>.Failure("تنها درخواست‌های در انتظار قابل لغو هستند.");
            }

            var updateDto = new UpdateRequestDto
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                Address = request.Address,
                CityId = request.CityId,
                DateRequired = request.DateRequired,
                Status = RequestStatus.Canceled,
                WinnerSuggestionId = request.WinnerSuggestionId
            };

            var result = await requestRepository.UpdateAsync(updateDto, ct);

            return result
                ? Result<bool>.Success(true, "درخواست لغو شد.")
                : Result<bool>.Failure("خطا در لغو درخواست.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error cancelling request | RequestId:{RequestId}",
                command.RequestId);

            return Result<bool>.Failure("خطای سیستمی رخ داده است.");
        }
    }
}
