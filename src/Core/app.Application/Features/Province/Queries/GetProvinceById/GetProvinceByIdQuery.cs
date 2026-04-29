using app.Application.Common;
using app.Application.DTOs.LocationDTOs;
using MediatR;

namespace app.Application.Features.Province.Queries.GetProvinceById;

public record GetProvinceByIdQuery(int Id) : IRequest<Result<ProvinceDto>>;
