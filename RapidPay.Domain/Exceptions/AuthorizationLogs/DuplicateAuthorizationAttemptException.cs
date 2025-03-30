namespace RapidPay.Domain.Exceptions.AuthorizationLogs
{
    public class DuplicateAuthorizationAttemptException : Exception
    {
        public DuplicateAuthorizationAttemptException() : base("A duplicate authorization attempt was detected.") { }
        public DuplicateAuthorizationAttemptException(Guid cardId, DateTime timestamp) : base($"A duplicate authorization attempt was detected (card id: {cardId}, timestamp: {timestamp}).") { }
        public DuplicateAuthorizationAttemptException(string cardNumber, DateTime timestamp) : base($"A duplicate authorization attempt was detected (card number: {cardNumber}, timestamp: {timestamp}).") { }
    }
}
