using RapidPay.Domain.Exceptions.Common;
using RapidPay.Domain.Interfaces.Infrastructure.Repositories.Common;

namespace RapidPay.Infrastructure.Persistence.Repositories.Common
{
    public class BaseRepository<T>(RapidPayDbContext context) : IRepository<T>
        where T : class
    {
        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            var entity = await context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                throw new EntityNotFoundException<T>(id);
            }

            return entity;
        }

        public virtual async Task AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            context.Set<T>().Update(entity);

            await Task.CompletedTask;
        }
    }
}
