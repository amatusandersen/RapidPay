using Moq;
using RapidPay.Application.UseCases.Commands.Cards.AuthorizeCard;
using RapidPay.Domain.Entities;
using RapidPay.Domain.Enums;
using RapidPay.Domain.Exceptions.AuthorizationLogs;
using RapidPay.Domain.Interfaces.Factories;
using RapidPay.Domain.Interfaces.Infrastructure;
using RapidPay.Domain.Interfaces.Infrastructure.Repositories;
using RapidPay.Infrastructure.Persistence.Specifications.AuthorizationLogs;
using RapidPay.Infrastructure.Persistence.Specifications.Cards;

namespace RapidPay.UnitTests.Commands.Cards
{
    public class AuthorizeCardCommandHandlerTests
    {
        private readonly Mock<IAuthorizationLogFactory> _logFactoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICardRepository> _cardRepoMock;
        private readonly Mock<IAuthorizationLogRepository> _authLogRepoMock;
        private readonly AuthorizeCardCommandHandler _handler;

        public AuthorizeCardCommandHandlerTests()
        {
            _logFactoryMock = new Mock<IAuthorizationLogFactory>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cardRepoMock = new Mock<ICardRepository>();
            _authLogRepoMock = new Mock<IAuthorizationLogRepository>();

            _unitOfWorkMock.Setup(u => u.CardRepository).Returns(_cardRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.AuthorizationLogRepository).Returns(_authLogRepoMock.Object);

            _handler = new AuthorizeCardCommandHandler(_logFactoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldAuthorize_WhenNoDuplicateExists()
        {
            // Arrange
            var testCardNumber = "123456789012345";
            var cardId = Guid.NewGuid();
            var testCard = new Card { Id = cardId, Number = testCardNumber, Status = CardStatus.Active };
            var command = new AuthorizeCardCommand(testCardNumber);

            _cardRepoMock
                .Setup(repo => repo.GetSingleAsync(It.IsAny<ActiveCardSpecification>()))
                .ReturnsAsync(testCard);

            _authLogRepoMock
                .Setup(repo => repo.GetSingleAsync(It.IsAny<DuplicateAuthorizationSpecification>()))
                .ReturnsAsync((AuthorizationLog)null!);

            var successLog = new AuthorizationLog
            {
                Id = Guid.NewGuid(),
                CardId = cardId,
                Timestamp = DateTime.UtcNow,
                IsAuthorized = true
            };
            _logFactoryMock
                .Setup(lf => lf.Create(testCard.Id, true, string.Empty))
                .Returns(successLog);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _logFactoryMock.Verify(lf => lf.Create(testCard.Id, true, string.Empty), Times.Once);
            _authLogRepoMock.Verify(repo => repo.AddAsync(successLog), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenDuplicateExists()
        {
            // Arrange
            var testCardNumber = "123456789012345";
            var cardId = Guid.NewGuid();
            var testCard = new Card { Id = cardId, Number = testCardNumber, Status = CardStatus.Active };
            var command = new AuthorizeCardCommand(testCardNumber);

            _cardRepoMock
                .Setup(repo => repo.GetSingleAsync(It.IsAny<ActiveCardSpecification>()))
                .ReturnsAsync(testCard);

            var recentLog = new AuthorizationLog
            {
                Id = Guid.NewGuid(),
                CardId = cardId,
                Timestamp = DateTime.UtcNow.AddSeconds(-3)
            };
            _authLogRepoMock
                .Setup(repo => repo.GetSingleAsync(It.IsAny<DuplicateAuthorizationSpecification>()))
                .ReturnsAsync(recentLog);

            var failureLog = new AuthorizationLog
            {
                Id = Guid.NewGuid(),
                CardId = cardId,
                Timestamp = DateTime.UtcNow,
                IsAuthorized = false
            };
            _logFactoryMock
                .Setup(lf => lf.Create(testCard.Id, false, It.IsAny<string>()))
                .Returns(failureLog);

            // Act & Assert
            await Assert.ThrowsAsync<DuplicateAuthorizationAttemptException>(() =>
                _handler.Handle(command, CancellationToken.None));

            _logFactoryMock.Verify(lf => lf.Create(testCard.Id, false, It.IsAny<string>()), Times.Once);
            _authLogRepoMock.Verify(repo => repo.AddAsync(failureLog), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
