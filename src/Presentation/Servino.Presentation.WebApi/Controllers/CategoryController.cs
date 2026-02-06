using Microsoft.AspNetCore.Mvc;
using Servino.Domain.Core._common;
using Servino.Domain.Core.CategoryAgg.Contracts.AppService;


namespace Servino.Presentation.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryAppService categoryAppService) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAllCategoriesAsync(
            [FromQuery] int pageNumber = 1,      
            [FromQuery] int pageSize = 10,       
            [FromQuery] string searchKey = "",
            CancellationToken ct = default)           
        {
            var result = await categoryAppService.GetAllAsync(
                new PaginationRequestDto
                {
                    SearchKey = searchKey,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                ct);

            if (result.Count > 0)
                return Ok(result); 
            return NotFound("دسته‌بندی‌ها یافت نشد."); 
        }

        [HttpGet("children/{parentId:int?}")]
        public async Task<IActionResult> GetCategoriesByParentIdAsync(int? parentId, CancellationToken ct = default)
        {
            var result = await categoryAppService.GetCategoriesByParentIdAsync(parentId, ct);

            if (result.Count > 0)
                return Ok(result);

            return NotFound("دسته‌بندی فرزند یافت نشد.");
        }

        [HttpGet("{categoryId:int}/services")]
        public async Task<IActionResult> GetServicesByCategoryIdAsync(int categoryId, CancellationToken ct = default)
        {
            var result = await categoryAppService.GetServicesByCategoryIdAsync(categoryId, ct);

            if (result.Count > 0)
                return Ok(result);

            return NotFound("خدماتی برای این دسته‌بندی یافت نشد.");
        }
    }
}



