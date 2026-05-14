using app.Application.Contracts.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Experts.Queries.GetExpertServicesByExpertId;

public record GetExpertServicesByExpertIdQuery(int ExpertId)
    : IRequest<List<ExpertServiceItemDto>>;
