using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.ExpertHomeServiceAgg;
using app.Application.Contracts.Contracts.Services.RequestAgg;
using app.Application.Contracts.Contracts.Services.UserAgg;
using app.Application.Contracts.DTOs.RequestDTOs;
using MediatR;

namespace app.Application.Features.Requests.Queries.GetAvailableRequestsForExpert;

public class GetAvailableRequestsForExpertQueryHandler(
    IRequestService requestService,
    IExpertService expertService,
    IExpertHomeServiceService expertHomeServiceService)
    : IRequestHandler<GetAvailableRequestsForExpertQuery, List<RequestSummaryDto>>
{
    public async Task<List<RequestSummaryDto>> Handle(GetAvailableRequestsForExpertQuery query, CancellationToken ct)
    {
        var expertProfile = await expertService.GetByUserIdAsync(query.ExpertId, ct);

        if (expertProfile == null || expertProfile.CityId == null)
            return [];

        var skillIds = await expertHomeServiceService.GetServiceIdsByExpertIdAsync(expertProfile.ExpertId, ct);

        if (skillIds == null || !skillIds.Any())
            return [];

        return await requestService.GetAvailableForExpertAsync(
            expertProfile.ExpertId,
            skillIds,
            expertProfile.CityId.Value,
            ct);
    }
}
