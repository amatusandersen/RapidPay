using RapidPay.Domain.Interfaces.Specifications.Common;
using System.Linq.Expressions;

namespace RapidPay.Domain.Interfaces.Infrastructure.Repositories.Common
{
    public interface IRepository<T>
        where T : class
    {
        Task<T> GetSingleAsync(Guid id);
        Task<T> GetSingleAsync(ISpecification<T> specification);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
    }
}
