using app.Application.Contracts.Common;
using MediatR;

namespace app.Application.Features.City.Commands.UpdateCity;

public record UpdateCityCommand(int Id, string Title, int ProvinceId) : IRequest<Result<bool>>;
