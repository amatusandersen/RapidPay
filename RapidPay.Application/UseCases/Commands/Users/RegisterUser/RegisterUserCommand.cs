using MediatR;

namespace RapidPay.Application.UseCases.Commands.Users.RegisterUser
{
    public class RegisterUserCommand(string username, string password) : IRequest<bool>
    {
        public string Username { get; } = username;
        public string Password { get; } = password;
    }
}
