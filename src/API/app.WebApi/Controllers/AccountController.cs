using app.Application.DTOs.IdentityDTOs;
using app.Application.Features.Users.Commands.LoginWithPassword;
using app.Application.Features.Users.Commands.RegisterUser;
using app.Application.Features.Users.Commands.SendOtpCommand;
using app.WebApi.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace app.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IMediator mediator) : ControllerBase
    {

        [HttpPost("login/password")]
        public async Task<IActionResult> LoginWithPassword([FromBody] LoginWithPasswordDto dto, CancellationToken ct)
        {
            var command = new LoginWithPasswordCommand( new LoginWithPassDto
                {
                    UserName = dto.UserName,
                    Password = dto.Password
                });

            var result = await mediator.Send(command, ct);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }



        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp(
            [FromBody] SendOtpDto dto,
            CancellationToken ct)
        {
            var command = new SendOtpCommand(dto);

            var result = await mediator.Send(command, ct);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest dto, CancellationToken ct)
        {
            var command = new RegisterUserCommand(
                new RegisterDto
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    Password = dto.Password,
                    Role = dto.Role
                });

            var result = await mediator.Send(command, ct);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

    }
}
