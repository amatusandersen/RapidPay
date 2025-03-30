using RapidPay.Domain.Entities;

namespace RapidPay.Domain.Interfaces.Factories
{
    public interface ICardFactory
    {
        Card Create(decimal initialBalance, decimal? creditLimit);
    }
}
