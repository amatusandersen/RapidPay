using RapidPay.Domain.Entities;
using RapidPay.Domain.Interfaces.Factories;

namespace RapidPay.Application.Services.Factories
{
    public class TransactionFactory : ITransactionFactory
    {
        public Transaction Create(string senderCardNumber, string recipientCardNumber, decimal transactionAmount, decimal feeAmount)
        {
            var entity = new Transaction
            {
                Id = Guid.NewGuid(),
                SenderCardNumber = senderCardNumber,
                RecipientCardNumber = recipientCardNumber,
                TransactionAmount = transactionAmount,
                FeeAmount = feeAmount,
                Timestamp = DateTime.UtcNow
            };

            return entity;
        }
    }
}
