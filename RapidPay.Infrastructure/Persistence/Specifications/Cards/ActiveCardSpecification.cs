using RapidPay.Domain.Entities;
using RapidPay.Domain.Enums;
using RapidPay.Infrastructure.Persistence.Specifications.Common;
using System.Linq.Expressions;

namespace RapidPay.Infrastructure.Persistence.Specifications.Cards
{
    public class ActiveCardSpecification(string cardNumber) : BaseSpecification<Card>
    {
        public override Expression<Func<Card, bool>> Criteria => 
            c => c.Number == cardNumber && c.Status == CardStatus.Active;
    }
}
