using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.DTOs.RequestDTOs;
using app.Domain.RequestAgg.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Requests.Commands.UpdateRequest;

public class UpdateRequestCommandHandler(
    IRequestRepository requestRepository,
    ILogger<UpdateRequestCommandHandler> logger,
    IValidator<UpdateRequestCommand> validator)
    : IRequestHandler<UpdateRequestCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(UpdateRequestCommand request, CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        try
        {
            var dto = new UpdateRequestDto
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                Address = request.Address,
                CityId = request.CityId,
                DateRequired = request.DateRequired
            };

            var ok = await requestRepository.UpdateAsync(dto, ct);

            return ok
                ? Result<bool>.Success(true, "درخواست با موفقیت بروزرسانی شد.")
                : Result<bool>.Failure("بروزرسانی انجام نشد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating request {Id}", request.Id);
            return Result<bool>.Failure("خطای سیستمی");
        }
    }
}
