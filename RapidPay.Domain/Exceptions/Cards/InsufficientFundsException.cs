namespace RapidPay.Domain.Exceptions.Cards
{
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException() : base("Insufficient funds.") { }
        public InsufficientFundsException(Guid cardId) : base($"Insufficient funds (card id: {cardId}).") { }
        public InsufficientFundsException(Guid cardId, decimal paymentAmount) : base($"Insufficient funds (card id: {cardId}, required balance: {paymentAmount}).") { }
    }
}
