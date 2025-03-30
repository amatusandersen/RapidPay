using RapidPay.Domain.Entities;
using RapidPay.Infrastructure.Persistence.Specifications.Common;
using System.Linq.Expressions;

namespace RapidPay.Infrastructure.Persistence.Specifications.Fees
{
    public class LatestFeeSpecification : BaseSpecification<Fee>
    {
        public override Expression<Func<Fee, object>> OrderByDescending => f => f.Timestamp;
        public override int? Take => 1;
    }
}
