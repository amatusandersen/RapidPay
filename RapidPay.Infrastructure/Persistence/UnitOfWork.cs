using RapidPay.Domain.Interfaces.Infrastructure;
using RapidPay.Domain.Interfaces.Infrastructure.Repositories;

namespace RapidPay.Infrastructure.Persistence
{
    public class UnitOfWork(
        RapidPayDbContext context,
        ICardRepository cardRepository,
        IAuthorizationLogRepository authorizationLogRepository,
        IFeeRepository feeRepository,
        ITransactionRepository transactionRepository,
        IManualCardUpdateRepository manualCardUpdateRepository
    ) : IUnitOfWork
    {
        public ICardRepository CardRepository { get => cardRepository; set => cardRepository = value; }
        public IAuthorizationLogRepository AuthorizationLogRepository { get => authorizationLogRepository; set => authorizationLogRepository = value; }
        public IFeeRepository FeeRepository { get => feeRepository; set => feeRepository = value; }
        public ITransactionRepository TransactionRepository { get => transactionRepository; set => transactionRepository = value; }
        public IManualCardUpdateRepository ManualCardUpdateRepository { get => manualCardUpdateRepository; set => manualCardUpdateRepository = value; }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
