using RapidPay.Domain.Entities;
using RapidPay.Domain.Interfaces.Factories;

namespace RapidPay.Application.Services.Factories
{
    public class TransactionFactory : ITransactionFactory
    {
        public Transaction Create(Card senderCard, Card recipientCard, decimal transactionAmount, decimal feeAmount)
        {
            var entity = new Transaction
            {
                Id = Guid.NewGuid(),
                SenderCardId = senderCard.Id,
                SenderCardNumber = senderCard.Number,
                RecipientCardId = recipientCard.Id,
                RecipientCardNumber = recipientCard.Number,
                TransactionAmount = transactionAmount,
                FeeAmount = feeAmount,
                Timestamp = DateTime.UtcNow
            };

            return entity;
        }
    }
}
