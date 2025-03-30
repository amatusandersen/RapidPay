using RapidPay.Domain.Entities;

namespace RapidPay.Domain.Interfaces.Factories
{
    public interface IUserFactory
    {
        User Create(string username, string password);
    }
}
