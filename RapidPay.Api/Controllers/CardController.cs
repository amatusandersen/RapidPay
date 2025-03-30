using MediatR;
using Microsoft.AspNetCore.Mvc;
using RapidPay.Application.UseCases.Commands.AuthorizeCard;
using RapidPay.Application.UseCases.Commands.CreateCard;
using RapidPay.Application.UseCases.Commands.PayWithCard;
using RapidPay.Application.UseCases.Commands.UpdateCard;
using RapidPay.Application.UseCases.Queries.GetCardBalance;

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

        [HttpPost("authorize")]
        public async Task<IActionResult> AuthorizeCard([FromBody] AuthorizeCardCommand command)
        {
            var result = await mediator.Send(command);

            return Ok(result);
        }

        [HttpPost("pay")]
        public async Task<IActionResult> PayWithCard([FromBody] PayWithCardCommand command)
        {
            var result = await mediator.Send(command);

            return Ok(result);
        }

        [HttpGet("{cardNumber}/balance")]
        public async Task<IActionResult> GetCardBalance(string cardNumber)
        {
            var query = new GetCardBalanceQuery(cardNumber);

            var result = await mediator.Send(query);

            return Ok(result);
        }

        [HttpPatch("manual-update")]
        public async Task<IActionResult> UpdateCard([FromBody] UpdateCardCommand command)
        {
            var result = await mediator.Send(command);

            return Ok(result);
        }
    }
}
