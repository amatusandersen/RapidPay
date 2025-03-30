using MediatR;

namespace RapidPay.Application.UseCases.Commands.PayWithCard
{
    public class PayWithCardCommand(string cardNumber, decimal paymentAmount, string recipientCardNumber) : IRequest<bool>
    {
        public string CardNumber { get; } = cardNumber;
        public decimal PaymentAmount { get; } = paymentAmount;
        public string RecipientCardNumber { get; } = recipientCardNumber;
    }
}
