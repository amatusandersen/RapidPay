using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidPay.Application.UseCases.Commands.Users.LoginUser;
using RapidPay.Application.UseCases.Commands.Users.RegisterUser;

namespace RapidPay.Api.Controllers
{
    /// <summary>
    /// Provides endpoints for user authentication including registration and login.
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Registers a new user with the provided credentials.
        /// </summary>
        /// <param name="command">The command containing the registration details such as username and password.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the success of the registration.</returns>
        /// <response code="200">Returns true if registration is successful.</response>
        /// <response code="400">Returns a bad request if the user already exists or input is invalid.</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await mediator.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Logs in a user and returns a JWT token.
        /// </summary>
        /// <param name="command">The command containing login details such as username and password.</param>
        /// <returns>An <see cref="IActionResult"/> containing a JWT token if login is successful.</returns>
        /// <response code="200">Returns a JWT token as a string.</response>
        /// <response code="401">Returns unauthorized if credentials are invalid.</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await mediator.Send(command);

            return Ok(result);
        }
    }
}
