using Moq;
using RapidPay.Application.UseCases.Commands.Cards.UpdateCard;
using RapidPay.Domain.Entities;
using RapidPay.Domain.Enums;
using RapidPay.Domain.Exceptions.Common;
using RapidPay.Domain.Interfaces.Factories;
using RapidPay.Domain.Interfaces.Infrastructure;
using RapidPay.Domain.Interfaces.Infrastructure.Repositories;
using RapidPay.Infrastructure.Persistence.Specifications.Cards;

namespace RapidPay.UnitTests.Commands.Cards
{
    public class UpdateCardCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IManualCardUpdateFactory> _updateFactoryMock;
        private readonly Mock<ICardRepository> _cardRepositoryMock;
        private readonly Mock<IManualCardUpdateRepository> _manualCardUpdateRepositoryMock;
        private readonly UpdateCardCommandHandler _handler;

        public UpdateCardCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _updateFactoryMock = new Mock<IManualCardUpdateFactory>();
            _cardRepositoryMock = new Mock<ICardRepository>();
            _manualCardUpdateRepositoryMock = new Mock<IManualCardUpdateRepository>();

            _unitOfWorkMock.Setup(u => u.CardRepository).Returns(_cardRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.ManualCardUpdateRepository).Returns(_manualCardUpdateRepositoryMock.Object);

            _handler = new UpdateCardCommandHandler(_unitOfWorkMock.Object, _updateFactoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowEntityNotFoundException_WhenCardNotFound()
        {
            // Arrange
            var command = new UpdateCardCommand("123456789012345", null, null, null);
            _cardRepositoryMock.Setup(r => r.GetSingleAsync(It.IsAny<GetCardByNumberSpecification>()))!
                               .ReturnsAsync((Card)null!);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException<Card>>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldUpdateCardAndLogChanges_WhenChangesAreMade()
        {
            // Arrange
            var cardNumber = "123456789012345";
            var cardId = Guid.NewGuid();
            var initialBalance = 100m;
            var initialCreditLimit = 50m;
            var initialStatus = CardStatus.Active;
            var card = new Card
            {
                Id = cardId,
                Number = cardNumber,
                Balance = initialBalance,
                CreditLimit = initialCreditLimit,
                Status = initialStatus
            };

            var newBalance = 120m;
            var newCreditLimit = 60m;
            var newStatus = CardStatus.Suspended;

            var command = new UpdateCardCommand(cardNumber, newBalance, newCreditLimit, newStatus);

            _cardRepositoryMock.Setup(r => r.GetSingleAsync(It.IsAny<GetCardByNumberSpecification>()))
                               .ReturnsAsync(card);

            var manualUpdateRecord = new ManualCardUpdate { Id = Guid.NewGuid(), CardId = cardId };
            _updateFactoryMock.Setup(f => f.Create(cardId, It.IsAny<List<string>>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, string>>()))
                              .Returns(manualUpdateRecord);

            _manualCardUpdateRepositoryMock.Setup(r => r.AddAsync(manualUpdateRecord))
                                           .Returns(Task.CompletedTask);
            _cardRepositoryMock.Setup(r => r.UpdateAsync(card)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(newBalance, card.Balance);
            Assert.Equal(newCreditLimit, card.CreditLimit);
            Assert.Equal(newStatus, card.Status);

            _updateFactoryMock.Verify(f => f.Create(cardId, It.IsAny<List<string>>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            _manualCardUpdateRepositoryMock.Verify(r => r.AddAsync(manualUpdateRecord), Times.Once);

            _cardRepositoryMock.Verify(r => r.UpdateAsync(card), Times.Once);

            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Exactly(2));

            var dto = result;
            Assert.Equal(card.Id, dto.Id);
            Assert.Equal(card.Number, dto.Number);
            Assert.Equal(card.Balance, dto.Balance);
            Assert.Equal(card.CreditLimit, dto.CreditLimit);
            Assert.Equal(card.Status, dto.Status);
        }

        [Fact]
        public async Task Handle_ShouldUpdateCardWithoutLogging_WhenNoChangesAreMade()
        {
            // Arrange
            var cardNumber = "123456789012345";
            var cardId = Guid.NewGuid();
            var initialBalance = 100m;
            var initialCreditLimit = 50m;
            var initialStatus = CardStatus.Active;

            var card = new Card
            {
                Id = cardId,
                Number = cardNumber,
                Balance = initialBalance,
                CreditLimit = initialCreditLimit,
                Status = initialStatus
            };

            var command = new UpdateCardCommand(cardNumber, null, initialCreditLimit, CardStatus.Active);

            _cardRepositoryMock.Setup(r => r.GetSingleAsync(It.IsAny<GetCardByNumberSpecification>()))
                               .ReturnsAsync(card);
            _cardRepositoryMock.Setup(r => r.UpdateAsync(card)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(initialBalance, card.Balance);
            Assert.Equal(initialCreditLimit, card.CreditLimit);
            Assert.Equal(initialStatus, card.Status);

            _updateFactoryMock.Verify(f => f.Create(It.IsAny<Guid>(), It.IsAny<List<string>>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            _manualCardUpdateRepositoryMock.Verify(r => r.AddAsync(It.IsAny<ManualCardUpdate>()), Times.Never);

            _cardRepositoryMock.Verify(r => r.UpdateAsync(card), Times.Once);

            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);

            var dto = result;
            Assert.Equal(card.Id, dto.Id);
            Assert.Equal(card.Number, dto.Number);
            Assert.Equal(card.Balance, dto.Balance);
            Assert.Equal(card.CreditLimit, dto.CreditLimit);
            Assert.Equal(card.Status, dto.Status);
        }
    }
}
