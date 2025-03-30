namespace RapidPay.Domain.Entities
{
    public class Fee
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
