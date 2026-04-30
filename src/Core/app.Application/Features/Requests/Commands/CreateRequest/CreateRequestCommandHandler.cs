using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.DTOs.RequestDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Requests.Commands.CreateRequest;

public class CreateRequestCommandHandler(
    IRequestRepository requestRepository,
    ILogger<CreateRequestCommandHandler> logger)
    : IRequestHandler<CreateRequestCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateRequestCommand request, CancellationToken ct)
    {
        try
        {
            var dto = new CreateRequestDto
            {
                Title = request.Title,
                Description = request.Description,
                Address = request.Address,
                CityId = request.CityId,
                CustomerId = request.CustomerId,
                HomeServiceId = request.HomeServiceId,
                DateRequired = request.DateRequired,
                ImagePaths = request.ImagePaths
            };

            var newId = await requestRepository.CreateAsync(dto, ct);

            return Result<int>.Success(newId, "درخواست با موفقیت ثبت شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error creating request | CustomerId:{CustomerId} | Service:{ServiceId}",
                request.CustomerId,
                request.HomeServiceId);

            return Result<int>.Failure("خطای سیستمی رخ داده است.");
        }
    }
}
