using RapidPay.Domain.Entities;
using RapidPay.Domain.Enums;
using RapidPay.Domain.Interfaces.Factories;

namespace RapidPay.Application.Services.Factories
{
    public class CardFactory(Random random) : ICardFactory
    {
        public Card Create(decimal initialBalance, decimal? creditLimit)
        {
            var cardNumber = GenerateCardNumber();

            var model = new Card
            {
                Id = Guid.NewGuid(),
                Balance = initialBalance,
                Number = cardNumber,
                CreditLimit = creditLimit,
                Status = CardStatus.Active,
                CreatedAt = DateTime.Now
            };

            return model;
        }

        private string GenerateCardNumber()
        {
            return string.Concat(Enumerable.Range(0, 15).Select(_ => random.Next(0, 10)));
        }
    }
}
