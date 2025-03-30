using Microsoft.EntityFrameworkCore;
using RapidPay.Domain.Interfaces.Infrastructure.Repositories.Common;
using RapidPay.Domain.Interfaces.Specifications.Common;
using System.Linq.Expressions;

namespace RapidPay.Infrastructure.Persistence.Repositories.Common
{
    public class BaseRepository<T>(RapidPayDbContext Context) : IRepository<T>
        where T : class
    {
        public virtual async Task<T> GetSingleAsync(Guid id)
        {
            var entity = await Context.Set<T>().FindAsync(id);

            return entity!;
        }

        public virtual async Task<T> GetSingleAsync(ISpecification<T> specification)
        {
            IQueryable<T> query = Context.Set<T>();
            if (specification != null)
            {
                if (specification.Criteria != null)
                {
                    query = query.Where(specification.Criteria);
                }

                foreach (var include in specification.Includes)
                {
                    query = query.Include(include);
                }

                if (specification.OrderBy != null)
                {
                    query = query.OrderBy(specification.OrderBy);
                }
                else if (specification.OrderByDescending != null)
                {
                    query = query.OrderByDescending(specification.OrderByDescending);
                }

                if (specification.Skip.HasValue)
                {
                    query = query.Skip(specification.Skip.Value);
                }

                if (specification.Take.HasValue)
                {
                    query = query.Take(specification.Take.Value);
                }
            }

            var entity = await query.FirstOrDefaultAsync();

            return entity!;
        }

        public virtual async Task AddAsync(T entity)
        {
            await Context.Set<T>().AddAsync(entity);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            Context.Set<T>().Update(entity);

            await Task.CompletedTask;
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> condition)
        {
            return await Context.Set<T>().AnyAsync(condition);
        }
    }
}
