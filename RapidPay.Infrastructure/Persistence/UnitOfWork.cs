using RapidPay.Domain.Interfaces.Infrastructure;
using RapidPay.Domain.Interfaces.Infrastructure.Repositories;

namespace RapidPay.Infrastructure.Persistence
{
    public class UnitOfWork(RapidPayDbContext context, ICardRepository cardRepository) 
        : IUnitOfWork
    {
        public ICardRepository CardRepository { get => cardRepository; set => cardRepository = value; }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
