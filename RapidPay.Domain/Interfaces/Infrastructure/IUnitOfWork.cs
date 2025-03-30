using RapidPay.Domain.Interfaces.Infrastructure.Repositories;

namespace RapidPay.Domain.Interfaces.Infrastructure
{
    public interface IUnitOfWork
    {
        ICardRepository CardRepository { get; set; }
        IAuthorizationLogRepository AuthorizationLogRepository { get; set; }
        Task SaveChangesAsync();
    }
}
