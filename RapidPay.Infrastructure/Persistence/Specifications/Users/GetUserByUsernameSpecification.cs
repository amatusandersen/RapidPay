using RapidPay.Domain.Entities;
using RapidPay.Infrastructure.Persistence.Specifications.Common;
using System.Linq.Expressions;

namespace RapidPay.Infrastructure.Persistence.Specifications.Users
{
    public class GetUserByUsernameSpecification(string username) : BaseSpecification<User>
    {
        public override Expression<Func<User, bool>> Criteria => u => u.Username == username;
    }
}
