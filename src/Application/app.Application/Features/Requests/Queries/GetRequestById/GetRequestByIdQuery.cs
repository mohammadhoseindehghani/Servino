using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.RequestDTOs;
using MediatR;

namespace app.Application.Features.Requests.Queries.GetRequestById;

public record GetRequestByIdQuery(int Id)
    : IRequest<Result<RequestFullDto>>;
