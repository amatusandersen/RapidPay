using RapidPay.Domain.Entities;
using RapidPay.Domain.Interfaces.Factories;

namespace RapidPay.Application.Services.Factories
{
    public class AuthorizationLogFactory : IAuthorizationLogFactory
    {
        public AuthorizationLog Create(Guid cardId, bool isAuthorized, string message)
        {
            var log = new AuthorizationLog
            {
                Id = Guid.NewGuid(),
                CardId = cardId,
                IsAuthorized = isAuthorized,
                Reason = message,
                Timestamp = DateTime.UtcNow
            };

            return log;
        }
    }
}
