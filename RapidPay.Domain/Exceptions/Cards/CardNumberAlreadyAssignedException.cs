namespace RapidPay.Domain.Exceptions.Cards
{
    public class CardNumberAlreadyAssignedException : Exception
    {
        public CardNumberAlreadyAssignedException() : base("Card number is already assigned to another card.") { }
        public CardNumberAlreadyAssignedException(string cardNumber) : base($"Card number is already assigned to another card (card number: {cardNumber}).") { }
    }
}
