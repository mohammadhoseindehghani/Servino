using app.Application.Common;
using MediatR;

namespace app.Application.Features.HomeServices.Commands.DeleteHomeService;

public record DeleteHomeServiceCommand(int Id) : IRequest<Result<bool>>;
