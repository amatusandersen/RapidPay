using MediatR;

namespace RapidPay.Application.UseCases.Commands.AuthorizeCard
{
    public class AuthorizeCardCommand(string cardNumber) : IRequest<bool>
    {
        public string CardNumber { get; } = cardNumber;
    }
}
