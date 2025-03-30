using MediatR;

namespace RapidPay.Application.UseCases.Commands.Users.LoginUser
{
    public class LoginUserCommand(string username, string password) : IRequest<string>
    {
        public string Username { get; } = username;
        public string Password { get; } = password;
    }
}
