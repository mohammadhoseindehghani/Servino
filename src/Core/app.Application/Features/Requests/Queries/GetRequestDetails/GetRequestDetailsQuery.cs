using app.Application.Common;
using app.Application.DTOs.RequestDTOs;
using MediatR;

namespace app.Application.Features.Requests.Queries.GetRequestDetails;

public record GetRequestDetailsQuery(int Id)
    : IRequest<Result<RequestDetailDto>>;
