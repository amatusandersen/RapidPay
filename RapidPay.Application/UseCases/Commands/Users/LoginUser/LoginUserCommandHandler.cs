using MediatR;
using Microsoft.AspNetCore.Identity;
using RapidPay.Domain.Entities;
using RapidPay.Domain.Exceptions.Users;
using RapidPay.Domain.Interfaces.Infrastructure;
using RapidPay.Domain.Interfaces.Services;
using RapidPay.Infrastructure.Persistence.Specifications.Users;

namespace RapidPay.Application.UseCases.Commands.Users.LoginUser
{
    public class LoginUserCommandHandler(IPasswordHasher<User> passwordHasher, ITokenService tokenService, IUnitOfWork unitOfWork) : IRequestHandler<LoginUserCommand, string>
    {
        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var userSpecification = new GetUserByUsernameSpecification(request.Username);
            var user = await unitOfWork.UserRepository.GetSingleAsync(userSpecification)
                ?? throw new UserIsNotRegisteredException(request.Username);

            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result != PasswordVerificationResult.Success)
            {
                throw new InvalidPasswordException();
            }

            var token = tokenService.GenerateToken(user);

            return token;
        }
    }
}
