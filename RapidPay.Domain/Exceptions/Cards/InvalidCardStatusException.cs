using RapidPay.Domain.Enums;

namespace RapidPay.Domain.Exceptions.Cards
{
    public class InvalidCardStatusException : Exception
    {
        public InvalidCardStatusException() : base("Invalid card status exception.") { }
        public InvalidCardStatusException(Guid id) : base($"Invalid card status exception (card id: {id}).") { }
        public InvalidCardStatusException(Guid id, CardStatus status) : base($"Invalid card status exception (card id: {id}, card status: {status}).") { }
    }
}
