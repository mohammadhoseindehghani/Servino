using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.DTOs.RequestDTOs;
using MediatR;

namespace app.Application.Features.Requests.Queries.GetAvailableRequestsForExpert;

public class GetAvailableRequestsForExpertQueryHandler(
    IRequestRepository requestRepository,
    IExpertRepository expertRepository,
    IExpertHomeServiceRepository expertHomeServiceRepository)
    : IRequestHandler<GetAvailableRequestsForExpertQuery, List<RequestSummaryDto>>
{
    public async Task<List<RequestSummaryDto>> Handle(GetAvailableRequestsForExpertQuery query, CancellationToken ct)
    {
        var expertProfile = await expertRepository.GetByUserIdAsync(query.ExpertId, ct);

        if (expertProfile == null || expertProfile.CityId == null)
            return [];

        var skillIds = await expertHomeServiceRepository.GetServiceIdsByExpertIdAsync(expertProfile.ExpertId, ct);

        if (skillIds == null || !skillIds.Any())
            return [];

        return await requestRepository.GetAvailableForExpertAsync(
            expertProfile.ExpertId,
            skillIds,
            expertProfile.CityId.Value,
            ct);
    }
}
