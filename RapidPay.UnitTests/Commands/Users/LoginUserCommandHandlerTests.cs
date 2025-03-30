using Microsoft.AspNetCore.Identity;
using Moq;
using RapidPay.Application.UseCases.Commands.Users.LoginUser;
using RapidPay.Domain.Entities;
using RapidPay.Domain.Exceptions.Users;
using RapidPay.Domain.Interfaces.Infrastructure;
using RapidPay.Domain.Interfaces.Infrastructure.Repositories;
using RapidPay.Domain.Interfaces.Services;
using RapidPay.Infrastructure.Persistence.Specifications.Users;

namespace RapidPay.UnitTests.Commands.Users
{
    public class LoginUserCommandHandlerTests
    {
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly LoginUserCommandHandler _handler;

        public LoginUserCommandHandlerTests()
        {
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();
            _tokenServiceMock = new Mock<ITokenService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();

            _unitOfWorkMock.Setup(u => u.UserRepository).Returns(_userRepositoryMock.Object);

            _handler = new LoginUserCommandHandler(_passwordHasherMock.Object, _tokenServiceMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsToken_WhenCredentialsAreValid()
        {
            // Arrange
            var username = "testuser";
            var password = "password123";
            var dummyHash = "hashed-password";
            var expectedToken = "dummy-token";
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                PasswordHash = dummyHash
            };

            var command = new LoginUserCommand(username, password);

            _userRepositoryMock
                .Setup(repo => repo.GetSingleAsync(It.IsAny<GetUserByUsernameSpecification>()))
                .ReturnsAsync(user);

            _passwordHasherMock
                .Setup(ph => ph.VerifyHashedPassword(user, dummyHash, password))
                .Returns(PasswordVerificationResult.Success);

            _tokenServiceMock
                .Setup(ts => ts.GenerateToken(user))
                .Returns(expectedToken);

            // Act
            var token = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(expectedToken, token);
            _userRepositoryMock.Verify(repo => repo.GetSingleAsync(It.IsAny<GetUserByUsernameSpecification>()), Times.Once);
            _passwordHasherMock.Verify(ph => ph.VerifyHashedPassword(user, dummyHash, password), Times.Once);
            _tokenServiceMock.Verify(ts => ts.GenerateToken(user), Times.Once);
        }

        [Fact]
        public async Task Handle_ThrowsUserIsNotRegisteredException_WhenUserNotFound()
        {
            // Arrange
            var username = "nonexistent";
            var password = "password123";
            var command = new LoginUserCommand(username, password);

            _userRepositoryMock
                .Setup(repo => repo.GetSingleAsync(It.IsAny<GetUserByUsernameSpecification>()))!
                .ReturnsAsync((User)null!);

            // Act & Assert
            await Assert.ThrowsAsync<UserIsNotRegisteredException>(() => _handler.Handle(command, CancellationToken.None));

            _userRepositoryMock.Verify(repo => repo.GetSingleAsync(It.IsAny<GetUserByUsernameSpecification>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ThrowsInvalidPasswordException_WhenPasswordIsInvalid()
        {
            // Arrange
            var username = "testuser";
            var password = "wrongpassword";
            var dummyHash = "hashed-password";
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                PasswordHash = dummyHash
            };

            var command = new LoginUserCommand(username, password);

            _userRepositoryMock
                .Setup(repo => repo.GetSingleAsync(It.IsAny<GetUserByUsernameSpecification>()))
                .ReturnsAsync(user);

            _passwordHasherMock
                .Setup(ph => ph.VerifyHashedPassword(user, dummyHash, password))
                .Returns(PasswordVerificationResult.Failed);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidPasswordException>(() => _handler.Handle(command, CancellationToken.None));

            _userRepositoryMock.Verify(repo => repo.GetSingleAsync(It.IsAny<GetUserByUsernameSpecification>()), Times.Once);
            _passwordHasherMock.Verify(ph => ph.VerifyHashedPassword(user, dummyHash, password), Times.Once);
        }
    }
}