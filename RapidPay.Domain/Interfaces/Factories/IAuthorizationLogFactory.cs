using RapidPay.Domain.Entities;

namespace RapidPay.Domain.Interfaces.Factories
{
    public interface IAuthorizationLogFactory
    {
        AuthorizationLog Create(Guid cardId, bool isAuthorized, string message);
    }
}
