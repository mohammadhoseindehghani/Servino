using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.DTOs.RequestDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Requests.Queries.GetRequestDetails;

public class GetRequestDetailsQueryHandler(
    IRequestRepository requestRepository,
    ILogger<GetRequestDetailsQueryHandler> logger)
    : IRequestHandler<GetRequestDetailsQuery, Result<RequestDetailDto>>
{

    public async Task<Result<RequestDetailDto>> Handle(GetRequestDetailsQuery query, CancellationToken ct)
    {
        try
        {
            var details = await requestRepository.GetDetailsByIdAsync(query.Id, ct);

            return details == null
                ? Result<RequestDetailDto>.Failure("جزئیات درخواست یافت نشد.", "404")
                : Result<RequestDetailDto>.Success(details);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "System error in RequestAppService.GetDetailsByIdAsync | RequestId: {RequestId}",
                query.Id);

            return Result<RequestDetailDto>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }
}
