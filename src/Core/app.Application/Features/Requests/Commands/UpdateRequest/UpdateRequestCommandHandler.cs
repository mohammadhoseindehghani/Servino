using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.DTOs.RequestDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Requests.Commands.UpdateRequest;

public class UpdateRequestCommandHandler(
    IRequestRepository requestRepository,
    ILogger<UpdateRequestCommandHandler> logger)
    : IRequestHandler<UpdateRequestCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(UpdateRequestCommand cmd, CancellationToken ct)
    {
        try
        {
            var dto = new UpdateRequestDto
            {
                Id = cmd.Id,
                Title = cmd.Title,
                Description = cmd.Description,
                Address = cmd.Address,
                CityId = cmd.CityId,
                DateRequired = cmd.DateRequired
            };

            var ok = await requestRepository.UpdateAsync(dto, ct);

            return ok
                ? Result<bool>.Success(true, "درخواست با موفقیت بروزرسانی شد.")
                : Result<bool>.Failure("بروزرسانی انجام نشد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating request {Id}", cmd.Id);
            return Result<bool>.Failure("خطای سیستمی");
        }
    }
}
