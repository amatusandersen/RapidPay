using RapidPay.Domain.Entities;

namespace RapidPay.Domain.Interfaces.Factories
{
    public interface ITransactionFactory
    {
        Transaction Create(string senderCardNumber, string recipientCardNumber, decimal transactionAmount, decimal feeAmount);
    }
}
