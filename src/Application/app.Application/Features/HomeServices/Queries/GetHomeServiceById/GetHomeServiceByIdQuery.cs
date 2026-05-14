using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.HomeServiceDTOs;
using MediatR;

namespace app.Application.Features.HomeServices.Queries.GetHomeServiceById;

public record GetHomeServiceByIdQuery(int Id) : IRequest<Result<HomeServiceDto>>;
