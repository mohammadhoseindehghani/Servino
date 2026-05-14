
using app.Application.Contracts.Contracts.Services;
using app.Application.Features.Categories.Commands.Create;
using app.Application.Features.Categories.Commands.Delete;
using app.Application.Features.Categories.Commands.Update;
using app.Application.Features.Categories.Queries.GetBreadcrumb;
using app.Application.Features.Categories.Queries.GetCategories;
using app.Application.Features.Categories.Queries.GetCategoriesByParentId;
using app.Application.Features.Categories.Queries.GetCategoryById;
using app.Application.Features.Categories.Queries.GetServicesByCategoryId;
using app.WebApi.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(IMediator mediator, IFileService fileService) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] CreateCategoryDto model, CancellationToken ct)
        {
            var command = new CreateCategoryCommand(
                model.Title,
                model.Image,
                model.ParentId
            );

            var result = await mediator.Send(command, ct);

            return result.IsSuccess
                ? CreatedAtAction(nameof(GetById), new { id = result.Data }, result.Data)
                : BadRequest(result.Message);
        }

        
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await mediator.Send(new DeleteCategoryCommand(id));

            if (!result.IsSuccess)
                return BadRequest(result.Message); 

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateCategoryDto model, CancellationToken ct) 
        {

            var command = new UpdateCategoryCommand(id, model.Title, model.Image, model.ParentId);
            var result = await mediator.Send(command, ct);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return NoContent(); 
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery]int pageNumber, [FromQuery] int pageSize, [FromQuery] string? search)
        {
            var result = await mediator.Send(new GetCategoriesQuery(pageNumber, pageSize, search));
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await mediator.Send(new GetCategoryByIdQuery(id));

            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("{parentId:int}/subcategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByParentId(int parentId)
        {
            var result = await mediator.Send(new GetCategoriesByParentIdQuery(parentId));

            return Ok(result);
        }

        [HttpGet("{id:int}/services")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetServicesByCategoryId(int id)
        {
            var result = await mediator.Send(new GetServicesByCategoryIdQuery(id));

            return Ok(result);
        }

        [HttpGet("{id:int}/breadcrumb")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBreadcrumb(int id) 
        {
            var result = await mediator.Send(new GetCategoryBreadcrumbQuery(id));

            return Ok(result);
        }

    }
}