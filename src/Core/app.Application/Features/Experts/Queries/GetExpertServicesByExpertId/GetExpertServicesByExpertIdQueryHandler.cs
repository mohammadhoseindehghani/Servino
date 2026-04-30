using app.Application.Contracts.Repositories;
using app.Application.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Experts.Queries.GetExpertServicesByExpertId;

public class GetExpertServicesByExpertIdQueryHandler(IExpertHomeServiceRepository expertHomeServiceRepository)
    : IRequestHandler<GetExpertServicesByExpertIdQuery, List<ExpertServiceItemDto>>
{
    public async Task<List<ExpertServiceItemDto>> Handle(
        GetExpertServicesByExpertIdQuery request,
        CancellationToken ct)
    {
        return await expertHomeServiceRepository
            .GetExpertServicesByExpertIdAsync(request.ExpertId, ct);
    }
}
