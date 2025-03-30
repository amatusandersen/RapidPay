using RapidPay.Domain.Enums;

namespace RapidPay.Application.Dtos
{
    public class CardModel
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public CardStatus Status { get; set; }
        public decimal Balance { get; set; }
        public decimal? CreditLimit { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
