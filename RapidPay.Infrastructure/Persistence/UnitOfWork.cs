using RapidPay.Domain.Interfaces.Infrastructure;
using RapidPay.Domain.Interfaces.Infrastructure.Repositories;

namespace RapidPay.Infrastructure.Persistence
{
    public class UnitOfWork(RapidPayDbContext context, ICardRepository cardRepository, IAuthorizationLogRepository authorizationLogRepository)
        : IUnitOfWork
    {
        public ICardRepository CardRepository { get => cardRepository; set => cardRepository = value; }
        public IAuthorizationLogRepository AuthorizationLogRepository { get => authorizationLogRepository; set => authorizationLogRepository = value; }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
