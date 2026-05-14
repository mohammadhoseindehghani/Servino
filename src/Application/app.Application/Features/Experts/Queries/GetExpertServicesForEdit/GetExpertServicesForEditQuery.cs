using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Experts.Queries.GetExpertServicesForEdit;

public record GetExpertServicesForEditQuery(int UserId)
    : IRequest<Result<List<ExpertServiceItemDto>>>;
