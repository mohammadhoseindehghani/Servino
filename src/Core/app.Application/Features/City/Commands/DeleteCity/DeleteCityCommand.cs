using app.Application.Common;
using MediatR;

namespace app.Application.Features.City.Commands.DeleteCity;

public record DeleteCityCommand(int Id) : IRequest<Result<bool>>;
