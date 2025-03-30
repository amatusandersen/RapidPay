using RapidPay.Domain.Entities;
using RapidPay.Domain.Interfaces.Infrastructure.Repositories.Common;

namespace RapidPay.Domain.Interfaces.Infrastructure.Repositories
{
    public interface IAuthorizationLogRepository : IRepository<AuthorizationLog>
    {
        Task LogAuthorization(Guid cardId, bool isAuthorized, string message);
    }
}
