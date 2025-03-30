namespace RapidPay.Domain.Entities
{
    public class AuthorizationLog
    {
        public Guid Id { get; set; }
        public Guid CardId { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsAuthorized { get; set; }
        public string Reason { get; set; }

        public Card Card { get; set; }
    }
}
