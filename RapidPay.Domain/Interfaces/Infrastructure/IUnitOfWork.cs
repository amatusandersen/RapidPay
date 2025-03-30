using RapidPay.Domain.Interfaces.Infrastructure.Repositories;

namespace RapidPay.Domain.Interfaces.Infrastructure
{
    public interface IUnitOfWork
    {
        ICardRepository CardRepository { get; set; }
        Task SaveChangesAsync();
    }
}
