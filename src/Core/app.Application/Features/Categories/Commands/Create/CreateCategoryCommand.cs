using app.Application.Common;
using app.Application.DTOs.CategoryDTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace app.Application.Features.Categories.Commands.Create;

public record CreateCategoryCommand(string Title, IFormFile? Image, int? ParentId) : IRequest<Result<int>>;
