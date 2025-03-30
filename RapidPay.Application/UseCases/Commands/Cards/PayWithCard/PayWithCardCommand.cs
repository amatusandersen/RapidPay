using MediatR;

namespace RapidPay.Application.UseCases.Commands.Cards.PayWithCard
{
    public class PayWithCardCommand(string senderCardNumber, string recipientCardNumber, decimal paymentAmount) : IRequest<bool>
    {
        public string CardNumber { get; } = senderCardNumber;
        public string RecipientCardNumber { get; } = recipientCardNumber;
        public decimal PaymentAmount { get; } = paymentAmount;
    }
}
