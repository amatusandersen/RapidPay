using Moq;
using RapidPay.Application.UseCases.Queries.Cards.GetCardBalance;
using RapidPay.Domain.Entities;
using RapidPay.Domain.Exceptions.Common;
using RapidPay.Domain.Interfaces.Infrastructure;
using RapidPay.Domain.Interfaces.Infrastructure.Repositories;
using RapidPay.Infrastructure.Persistence.Specifications.Cards;

namespace RapidPay.UnitTests.Queries.Cards
{
    public class GetCardBalanceQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICardRepository> _cardRepositoryMock;
        private readonly GetCardBalanceQueryHandler _handler;

        public GetCardBalanceQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cardRepositoryMock = new Mock<ICardRepository>();

            _unitOfWorkMock.Setup(u => u.CardRepository).Returns(_cardRepositoryMock.Object);

            _handler = new GetCardBalanceQueryHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsCardBalanceModel_WhenCardExists()
        {
            // Arrange
            var cardNumber = "123456789012345";
            var card = new Card
            {
                Id = Guid.NewGuid(),
                Number = cardNumber,
                Balance = 200m,
                CreditLimit = 50m
            };

            _cardRepositoryMock
                .Setup(repo => repo.GetSingleAsync(It.IsAny<GetCardByNumberSpecification>()))
                .ReturnsAsync(card);

            var query = new GetCardBalanceQuery(cardNumber);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200m, result.Balance);
            Assert.Equal(50m, result.CreditLimit);
        }

        [Fact]
        public async Task Handle_ThrowsEntityNotFoundException_WhenCardDoesNotExist()
        {
            // Arrange
            var cardNumber = "123456789012345";

            _cardRepositoryMock
                .Setup(repo => repo.GetSingleAsync(It.IsAny<GetCardByNumberSpecification>()))!
                .ReturnsAsync((Card)null!);

            var query = new GetCardBalanceQuery(cardNumber);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException<Card>>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}
