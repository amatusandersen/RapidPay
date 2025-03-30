using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidPay.Application.UseCases.Commands.Cards.AuthorizeCard;
using RapidPay.Application.UseCases.Commands.Cards.CreateCard;
using RapidPay.Application.UseCases.Commands.Cards.PayWithCard;
using RapidPay.Application.UseCases.Commands.Cards.UpdateCard;
using RapidPay.Application.UseCases.Queries.Cards.GetCardBalance;

namespace RapidPay.Api.Controllers
{
    /// <summary>
    /// Provides endpoints for managing cards, including creation, authorization, payment processing, balance retrieval, and manual updates.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CardController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Creates a new card with the provided initial balance and credit limit.
        /// </summary>
        /// <param name="command">The command containing the card creation details.</param>
        /// <returns>An <see cref="IActionResult"/> containing the created card model.</returns>
        /// <response code="200">Returns the created card details.</response>
        [HttpPost("create")]
        public async Task<IActionResult> CreateCard([FromBody] CreateCardCommand command)
        {
            var result = await mediator.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Authorizes a card by validating its status and logging the authorization attempt.
        /// </summary>
        /// <param name="command">The command containing the card number for authorization.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the outcome of the authorization.</returns>
        /// <response code="200">Returns a confirmation of successful authorization.</response>
        [HttpPost("authorize")]
        public async Task<IActionResult> AuthorizeCard([FromBody] AuthorizeCardCommand command)
        {
            var result = await mediator.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Processes a payment using an authorized card, applies payment fees, and updates card balances.
        /// </summary>
        /// <param name="command">The command containing payment details including sender and recipient card numbers and payment amount.</param>
        /// <returns>An <see cref="IActionResult"/> indicating whether the payment was successfully processed.</returns>
        /// <response code="200">Returns true if the payment was processed successfully.</response>
        [HttpPost("pay")]
        public async Task<IActionResult> PayWithCard([FromBody] PayWithCardCommand command)
        {
            var result = await mediator.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves the current balance and credit limit for the specified card.
        /// </summary>
        /// <param name="cardNumber">The unique card number.</param>
        /// <returns>An <see cref="IActionResult"/> containing the card balance and credit limit.</returns>
        /// <response code="200">Returns the card balance details.</response>
        [HttpGet("{cardNumber}/balance")]
        public async Task<IActionResult> GetCardBalance(string cardNumber)
        {
            var query = new GetCardBalanceQuery(cardNumber);

            var result = await mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Updates the card details such as balance, credit limit, or status. Changes are logged for audit purposes.
        /// </summary>
        /// <param name="command">The command containing the card number and new details for update.</param>
        /// <returns>An <see cref="IActionResult"/> containing the updated card model.</returns>
        /// <response code="200">Returns the updated card details.</response>
        [HttpPatch("manual-update")]
        public async Task<IActionResult> UpdateCard([FromBody] UpdateCardCommand command)
        {
            var result = await mediator.Send(command);

            return Ok(result);
        }
    }
}
