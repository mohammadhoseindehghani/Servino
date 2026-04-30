using app.Application.Common;
using app.Application.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Experts.Queries.GetExpertServicesForEdit;

public record GetExpertServicesForEditQuery(int UserId)
    : IRequest<Result<List<ExpertServiceItemDto>>>;
