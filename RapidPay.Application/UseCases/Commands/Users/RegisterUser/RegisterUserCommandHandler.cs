using MediatR;
using RapidPay.Domain.Exceptions.Users;
using RapidPay.Domain.Interfaces.Factories;
using RapidPay.Domain.Interfaces.Infrastructure;
using RapidPay.Infrastructure.Persistence.Specifications.Users;

namespace RapidPay.Application.UseCases.Commands.Users.RegisterUser
{
    public class RegisterUserCommandHandler(IUserFactory userFactory, IUnitOfWork unitOfWork) : IRequestHandler<RegisterUserCommand, bool>
    {
        public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUserSpecification = new GetUserByUsernameSpecification(request.Username);
            var existingUser = await unitOfWork.UserRepository.GetSingleAsync(existingUserSpecification);
            if (existingUser != null)
            {
                throw new UserAlreadyExistsException(request.Username);
            }

            var user = userFactory.Create(request.Username, request.Password);

            await unitOfWork.UserRepository.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
