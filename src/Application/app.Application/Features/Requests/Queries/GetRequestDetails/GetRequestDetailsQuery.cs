using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.RequestDTOs;
using MediatR;

namespace app.Application.Features.Requests.Queries.GetRequestDetails;

public record GetRequestDetailsQuery(int Id)
    : IRequest<Result<RequestDetailDto>>;
