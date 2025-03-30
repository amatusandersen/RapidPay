namespace RapidPay.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid SenderCardId { get; set; }
        public Guid RecipientCardId { get; set; }
        public string SenderCardNumber { get; set; }
        public string RecipientCardNumber { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal FeeAmount { get; set; }
        public DateTime Timestamp { get; set; }

        public Card SenderCard { get; set; }
        public Card RecipientCard { get; set; }
    }
}
