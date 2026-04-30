using app.Application.Common;
using app.Application.DTOs.RequestDTOs;
using MediatR;

namespace app.Application.Features.Requests.Queries.GetRequestById;

public record GetRequestByIdQuery(int Id)
    : IRequest<Result<RequestFullDto>>;
