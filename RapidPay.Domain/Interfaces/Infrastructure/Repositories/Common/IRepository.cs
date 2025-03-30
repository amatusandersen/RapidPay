namespace RapidPay.Domain.Interfaces.Infrastructure.Repositories.Common
{
    public interface IRepository<T>
        where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
    }
}
