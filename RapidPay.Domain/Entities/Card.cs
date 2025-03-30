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
    }
}
