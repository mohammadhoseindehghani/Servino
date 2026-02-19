using Microsoft.AspNetCore.Mvc;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Dtos.Identity;

namespace Servino.Presentation.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IUserAppService userAppService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterDto registerDto, CancellationToken ct)
        {
            var result = await userAppService.RegisterUserAsync(registerDto, ct);

            if (result.IsSuccess)
                return Ok(result); 
            return BadRequest(result); 
        }
    }
}
