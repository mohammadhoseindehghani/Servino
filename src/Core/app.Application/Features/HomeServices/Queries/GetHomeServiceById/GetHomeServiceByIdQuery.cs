using app.Application.Common;
using app.Application.DTOs.HomeServiceDTOs;
using MediatR;

namespace app.Application.Features.HomeServices.Queries.GetHomeServiceById;

public record GetHomeServiceByIdQuery(int Id) : IRequest<Result<HomeServiceDto>>;
