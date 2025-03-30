using Microsoft.AspNetCore.Identity;
using RapidPay.Domain.Entities;
using RapidPay.Domain.Interfaces.Factories;

namespace RapidPay.Application.Services.Factories
{
    public class UserFactory(IPasswordHasher<User> hasher) : IUserFactory
    {
        public User Create(string username, string password)
        {
            var entity = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
            };

            var hashedPassword = hasher.HashPassword(entity, password);

            entity.PasswordHash = hashedPassword;

            return entity;
        }
    }
}
