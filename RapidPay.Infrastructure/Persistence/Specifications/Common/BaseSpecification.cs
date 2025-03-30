using RapidPay.Domain.Interfaces.Specifications.Common;
using System.Linq.Expressions;

namespace RapidPay.Infrastructure.Persistence.Specifications.Common
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public virtual Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; } = [];
        public virtual Expression<Func<T, object>> OrderBy { get; }
        public virtual Expression<Func<T, object>> OrderByDescending { get; }
        public virtual int? Take { get; }
        public virtual int? Skip { get; }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
    }
}
