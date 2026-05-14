using app.Application.Contracts.Common;
using MediatR;

namespace app.Application.Features.Categories.Commands.Delete;

public record DeleteCategoryCommand(int Id) : IRequest<Result>;