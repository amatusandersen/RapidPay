using Moq;
using RapidPay.Application.UseCases.Commands.Users.RegisterUser;
using RapidPay.Domain.Entities;
using RapidPay.Domain.Exceptions.Users;
using RapidPay.Domain.Interfaces.Factories;
using RapidPay.Domain.Interfaces.Infrastructure;
using RapidPay.Domain.Interfaces.Infrastructure.Repositories;
using RapidPay.Infrastructure.Persistence.Specifications.Users;

namespace RapidPay.UnitTests.Commands.Users
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<IUserFactory> _userFactoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly RegisterUserCommandHandler _handler;

        public RegisterUserCommandHandlerTests()
        {
            _userFactoryMock = new Mock<IUserFactory>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _unitOfWorkMock.Setup(u => u.UserRepository).Returns(_userRepositoryMock.Object);

            _handler = new RegisterUserCommandHandler(_userFactoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenUserDoesNotExist()
        {
            // Arrange
            var username = "testuser";
            var password = "password123";
            var command = new RegisterUserCommand(username, password);

            _userRepositoryMock
                .Setup(repo => repo.GetSingleAsync(It.IsAny<GetUserByUsernameSpecification>()))!
                .ReturnsAsync((User)null!);

            var newUser = new User { Id = Guid.NewGuid(), Username = username, PasswordHash = "hashedPassword" };
            _userFactoryMock
                .Setup(uf => uf.Create(username, password))
                .Returns(newUser);

            _userRepositoryMock
                .Setup(repo => repo.AddAsync(newUser))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _userRepositoryMock.Verify(repo => repo.GetSingleAsync(It.IsAny<GetUserByUsernameSpecification>()), Times.Once);
            _userFactoryMock.Verify(uf => uf.Create(username, password), Times.Once);
            _userRepositoryMock.Verify(repo => repo.AddAsync(newUser), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserAlreadyExists()
        {
            // Arrange
            var username = "testuser";
            var password = "password123";
            var command = new RegisterUserCommand(username, password);

            var existingUser = new User { Id = Guid.NewGuid(), Username = username, PasswordHash = "hashedPassword" };
            _userRepositoryMock
                .Setup(repo => repo.GetSingleAsync(It.IsAny<GetUserByUsernameSpecification>()))
                .ReturnsAsync(existingUser);

            // Act & Assert
            await Assert.ThrowsAsync<UserAlreadyExistsException>(() => _handler.Handle(command, CancellationToken.None));

            _userRepositoryMock.Verify(repo => repo.GetSingleAsync(It.IsAny<GetUserByUsernameSpecification>()), Times.Once);
            _userFactoryMock.Verify(uf => uf.Create(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }
    }
}