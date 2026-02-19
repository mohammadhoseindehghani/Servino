using Microsoft.AspNetCore.Mvc;
using Servino.Domain.Core._common;
using Servino.Domain.Core.HomeServiceAgg.Contracts.AppService;


namespace Servino.Presentation.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeServiceController(IHomeServiceAppService homeServiceAppService) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAllHomeServicesAsync(
            [FromQuery] int pageNumber = 1,      
            [FromQuery] int pageSize = 10,      
            [FromQuery] string searchKey = "",   
            CancellationToken ct = default)    
        {
            var result = await homeServiceAppService.GetAllAsync(
                new PaginationRequestDto
                {
                    SearchKey = searchKey,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                ct);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message ?? "خطایی در دریافت خدمات منزل رخ داد.");
            }

            if (result.Data != null && result.Data.Count > 0)
            {
                return Ok(result.Data);
            }

            return NotFound("خدمات منزل یافت نشد.");
        }
    }
}
