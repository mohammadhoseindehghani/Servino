using app.Application.Contracts.Common;
using MediatR;

namespace app.Application.Features.City.Commands.CreateCity;

public record CreateCityCommand(string Title, int ProvinceId) : IRequest<Result<bool>>;
