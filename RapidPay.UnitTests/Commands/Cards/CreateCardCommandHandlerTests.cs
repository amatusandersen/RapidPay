using Moq;
using RapidPay.Application.Mappings;
using RapidPay.Application.UseCases.Commands.Cards.CreateCard;
using RapidPay.Domain.Entities;
using RapidPay.Domain.Enums;
using RapidPay.Domain.Exceptions.Cards;
using RapidPay.Domain.Interfaces.Factories;
using RapidPay.Domain.Interfaces.Infrastructure;
using RapidPay.Domain.Interfaces.Infrastructure.Repositories;
using System.Linq.Expressions;

namespace RapidPay.UnitTests.Commands.Cards
{
    public class CreateCardCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICardFactory> _cardFactoryMock;
        private readonly Mock<ICardRepository> _cardRepoMock;
        private readonly CreateCardCommandHandler _handler;

        public CreateCardCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cardFactoryMock = new Mock<ICardFactory>();
            _cardRepoMock = new Mock<ICardRepository>();

            _unitOfWorkMock.Setup(u => u.CardRepository).Returns(_cardRepoMock.Object);

            _handler = new CreateCardCommandHandler(_unitOfWorkMock.Object, _cardFactoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateCard_WhenCardDoesNotExist()
        {
            // Arrange
            var initialBalance = 100m;
            var creditLimit = 50m;
            var command = new CreateCardCommand(initialBalance, creditLimit);

            var card = new Card
            {
                Id = Guid.NewGuid(),
                Number = "123456789012345",
                Balance = initialBalance,
                CreditLimit = creditLimit,
                Status = CardStatus.Active
            };

            var expectedCardModel = card.ToDto();

            _cardFactoryMock
                .Setup(cf => cf.Create(initialBalance, creditLimit))
                .Returns(card);

            _cardRepoMock
                .Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Card, bool>>>()))
                .ReturnsAsync(false);

            _cardRepoMock
                .Setup(repo => repo.AddAsync(card))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCardModel.Number, result.Number);
            Assert.Equal(expectedCardModel.Balance, result.Balance);
            Assert.Equal(expectedCardModel.CreditLimit, result.CreditLimit);

            _cardFactoryMock.Verify(cf => cf.Create(initialBalance, creditLimit), Times.Once);
            _cardRepoMock.Verify(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Card, bool>>>()), Times.Once);
            _cardRepoMock.Verify(repo => repo.AddAsync(card), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenCardAlreadyExists()
        {
            // Arrange
            var initialBalance = 100m;
            var creditLimit = 50m;
            var command = new CreateCardCommand(initialBalance, creditLimit);

            var card = new Card
            {
                Id = Guid.NewGuid(),
                Number = "123456789012345",
                Balance = initialBalance,
                CreditLimit = creditLimit,
                Status = CardStatus.Active
            };

            _cardFactoryMock
                .Setup(cf => cf.Create(initialBalance, creditLimit))
                .Returns(card);

            _cardRepoMock
                .Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Card, bool>>>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<CardNumberAlreadyAssignedException>(() => _handler.Handle(command, CancellationToken.None));

            _cardFactoryMock.Verify(cf => cf.Create(initialBalance, creditLimit), Times.Once);
            _cardRepoMock.Verify(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Card, bool>>>()), Times.Once);

            _cardRepoMock.Verify(repo => repo.AddAsync(It.IsAny<Card>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }
    }
}
