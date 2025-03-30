namespace RapidPay.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public string SenderCardNumber { get; set; }
        public string RecipientCardNumber { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal FeeAmount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
