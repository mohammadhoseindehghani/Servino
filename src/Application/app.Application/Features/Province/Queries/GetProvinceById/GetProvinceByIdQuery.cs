using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.LocationDTOs;
using MediatR;

namespace app.Application.Features.Province.Queries.GetProvinceById;

public record GetProvinceByIdQuery(int Id) : IRequest<Result<ProvinceDto>>;
