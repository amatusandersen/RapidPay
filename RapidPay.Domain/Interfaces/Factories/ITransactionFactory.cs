using RapidPay.Domain.Entities;

namespace RapidPay.Domain.Interfaces.Factories
{
    public interface ITransactionFactory
    {
        Transaction Create(Card senderCard, Card recipientCard, decimal transactionAmount, decimal feeAmount);
    }
}
