using MediatR;
using Microsoft.AspNetCore.Mvc;
using RapidPay.Application.UseCases.Commands.CreateCard;

namespace RapidPay.Api.Controllers
{
    [ApiController]
    public class CardController(IMediator mediator) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateCard([FromBody] CreateCardCommand command)
        {
            var result = await mediator.Send(command);

            return Ok(result);
        }

        // TODO
        [HttpPost("authorize")]
        public async Task<IActionResult> AuthorizeCard()
        {
            return Ok();
        }

        // TODO
        [HttpPost("pay")]
        public async Task<IActionResult> PayWithCard()
        {
            return Ok();
        }

        // TODO
        [HttpGet("{cardNumber}/balance")]
        public async Task<IActionResult> GetCardBalance()
        {
            return Ok();
        }

        // TODO
        [HttpPut("")]
        public async Task<IActionResult> UpdateCard()
        {
            return Ok();
        }
    }
}
