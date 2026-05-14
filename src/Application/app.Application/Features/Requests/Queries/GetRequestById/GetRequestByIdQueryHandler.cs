using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.DTOs.RequestDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Requests.Queries.GetRequestById;

public class GetRequestByIdQueryHandler(
    IRequestRepository requestRepository,
    ILogger<GetRequestByIdQueryHandler> logger)
    : IRequestHandler<GetRequestByIdQuery, Result<RequestFullDto>>
{

    public async Task<Result<RequestFullDto>> Handle(GetRequestByIdQuery query, CancellationToken ct)
    {
        try
        {
            var request = await requestRepository.GetByIdAsync(query.Id, ct);

            return request == null
                ? Result<RequestFullDto>.Failure("درخواست یافت نشد.", "404")
                : Result<RequestFullDto>.Success(request);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading request {Id}", query.Id);
            return Result<RequestFullDto>.Failure("خطای سیستمی رخ داده است.");
        }
    }
}
