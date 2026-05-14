using app.Application.Contracts.Common;
using MediatR;

namespace app.Application.Features.HomeServices.Commands.DeleteHomeService;

public record DeleteHomeServiceCommand(int Id) : IRequest<Result<bool>>;
