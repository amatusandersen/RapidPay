namespace RapidPay.Application.Dtos
{
    public class CardBalanceModel
    {
        public decimal Balance { get; set; }
        public decimal? CreditLimit { get; set; }
    }
}
