using RapidPay.Domain.Entities;
using RapidPay.Domain.Interfaces.Infrastructure.Repositories;
using RapidPay.Infrastructure.Persistence.Repositories.Common;

namespace RapidPay.Infrastructure.Persistence.Repositories
{
    public class AuthorizationLogRepository(RapidPayDbContext context)
        : BaseRepository<AuthorizationLog>(context), IAuthorizationLogRepository
    {
        public async Task LogAuthorization(Guid cardId, bool isAuthorized, string message)
        {
            var log = new AuthorizationLog
            {
                Id = Guid.NewGuid(),
                CardId = cardId,
                IsAuthorized = isAuthorized,
                Reason = message,
                Timestamp = DateTime.UtcNow
            };

            await context.AddAsync(log);
        }
    }
}
