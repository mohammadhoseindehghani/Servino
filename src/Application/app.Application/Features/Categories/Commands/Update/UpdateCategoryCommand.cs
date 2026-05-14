using app.Application.Contracts.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace app.Application.Features.Categories.Commands.Update;

public record UpdateCategoryCommand(int Id, string Title, IFormFile? Image, int? ParentId)
    : IRequest<Result>;
