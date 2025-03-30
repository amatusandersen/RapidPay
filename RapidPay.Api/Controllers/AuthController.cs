using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidPay.Application.UseCases.Commands.Users.LoginUser;
using RapidPay.Application.UseCases.Commands.Users.RegisterUser;

namespace RapidPay.Api.Controllers
{
    [ApiController]
    [AllowAnonymous]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await mediator.Send(command);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await mediator.Send(command);

            return Ok(result);
        }
    }
}
