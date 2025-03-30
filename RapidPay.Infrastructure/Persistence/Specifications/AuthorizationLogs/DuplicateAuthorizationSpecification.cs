using RapidPay.Domain.Entities;
using RapidPay.Infrastructure.Persistence.Specifications.Common;
using System.Linq.Expressions;

namespace RapidPay.Infrastructure.Persistence.Specifications.AuthorizationLogs
{
    public class DuplicateAuthorizationSpecification(Guid CardId) : BaseSpecification<AuthorizationLog>
    {
        public override Expression<Func<AuthorizationLog, bool>> Criteria =>
            l => l.CardId == CardId;
        public override Expression<Func<AuthorizationLog, object>> OrderByDescending => l => l.Timestamp;

        public override int? Take => 1;
    }
}
