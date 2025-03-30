using RapidPay.Domain.Entities;

namespace RapidPay.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
