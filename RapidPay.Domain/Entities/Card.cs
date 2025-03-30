using RapidPay.Domain.Enums;

namespace RapidPay.Domain.Entities
{
    public class Card
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public CardStatus Status { get; set; }
        public decimal Balance { get; set; }
        public decimal? CreditLimit { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<AuthorizationLog> AuthorizationLogs { get; set; }
        public List<ManualCardUpdate> ManualUpdates { get; set; }

        public List<Transaction> OutcomingTransactions { get; set; }
        public List<Transaction> IncomingTransactions { get; set; }
    }
}
