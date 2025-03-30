using MediatR;
using Moq;
using RapidPay.Application.UseCases.Commands.Cards.AuthorizeCard;
using RapidPay.Application.UseCases.Commands.Cards.PayWithCard;
using RapidPay.Domain.Entities;
using RapidPay.Domain.Enums;
using RapidPay.Domain.Exceptions.Cards;
using RapidPay.Domain.Interfaces.Factories;
using RapidPay.Domain.Interfaces.Infrastructure;
using RapidPay.Domain.Interfaces.Infrastructure.Repositories;
using RapidPay.Infrastructure.Persistence.Specifications.Cards;
using RapidPay.Infrastructure.Persistence.Specifications.Fees;

namespace RapidPay.UnitTests.Commands.Cards
{
    public class PayWithCardCommandHandlerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ITransactionFactory> _transactionFactoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICardRepository> _cardRepositoryMock;
        private readonly Mock<IFeeRepository> _feeRepositoryMock;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly PayWithCardCommandHandler _handler;

        public PayWithCardCommandHandlerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _transactionFactoryMock = new Mock<ITransactionFactory>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cardRepositoryMock = new Mock<ICardRepository>();
            _feeRepositoryMock = new Mock<IFeeRepository>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();

            _unitOfWorkMock.Setup(u => u.CardRepository).Returns(_cardRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.FeeRepository).Returns(_feeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.TransactionRepository).Returns(_transactionRepositoryMock.Object);

            _handler = new PayWithCardCommandHandler(
                _mediatorMock.Object,
                _transactionFactoryMock.Object,
                _unitOfWorkMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldProcessPaymentSuccessfully()
        {
            // Arrange
            var senderNumber = "1111222233334444";
            var recipientNumber = "5555666677778888";
            var paymentAmount = 100m;
            var feeMultiplier = 0.1m;
            var fee = paymentAmount * feeMultiplier;
            var totalPaymentAmount = paymentAmount + fee;

            var senderCard = new Card
            {
                Id = Guid.NewGuid(),
                Number = senderNumber,
                Balance = 200m,
                CreditLimit = 50m,
                Status = CardStatus.Active
            };

            var recipientCard = new Card
            {
                Id = Guid.NewGuid(),
                Number = recipientNumber,
                Balance = 50m,
                CreditLimit = null,
                Status = CardStatus.Active
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<AuthorizeCardCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _cardRepositoryMock
                .Setup(repo => repo.GetSingleAsync(It.IsAny<ActiveCardSpecification>()))!
                .ReturnsAsync((ActiveCardSpecification spec) =>
                {
                    return spec.Criteria.Compile()(senderCard) ? senderCard :
                           spec.Criteria.Compile()(recipientCard) ? recipientCard : null;
                });

            var feeRecord = new Fee
            {
                Id = Guid.NewGuid(),
                Amount = feeMultiplier,
                Timestamp = DateTime.UtcNow
            };
            _feeRepositoryMock
                .Setup(repo => repo.GetSingleAsync(It.IsAny<LatestFeeSpecification>()))
                .ReturnsAsync(feeRecord);

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                SenderCardId = senderCard.Id,
                RecipientCardId = recipientCard.Id,
                TransactionAmount = totalPaymentAmount,
                FeeAmount = fee,
                Timestamp = DateTime.UtcNow
            };
            _transactionFactoryMock
                .Setup(tf => tf.Create(senderCard, recipientCard, totalPaymentAmount, fee))
                .Returns(transaction);

            _transactionRepositoryMock.Setup(tr => tr.AddAsync(transaction)).Returns(Task.CompletedTask);
            _cardRepositoryMock.Setup(repo => repo.UpdateAsync(senderCard)).Returns(Task.CompletedTask);
            _cardRepositoryMock.Setup(repo => repo.UpdateAsync(recipientCard)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            var command = new PayWithCardCommand(senderNumber, recipientNumber, paymentAmount);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Equal(200m - totalPaymentAmount, senderCard.Balance);
            Assert.Equal(50m + paymentAmount, recipientCard.Balance);

            _mediatorMock.Verify(m => m.Send(It.IsAny<AuthorizeCardCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _transactionFactoryMock.Verify(tf => tf.Create(senderCard, recipientCard, totalPaymentAmount, fee), Times.Once);
            _transactionRepositoryMock.Verify(tr => tr.AddAsync(transaction), Times.Once);
            _cardRepositoryMock.Verify(repo => repo.UpdateAsync(senderCard), Times.Once);
            _cardRepositoryMock.Verify(repo => repo.UpdateAsync(recipientCard), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowInsufficientFundsException_WhenSenderHasInsufficientFunds()
        {
            // Arrange
            var senderNumber = "1111222233334444";
            var recipientNumber = "5555666677778888";
            var paymentAmount = 100m;
            var feeMultiplier = 0.1m;
            var fee = paymentAmount * feeMultiplier;
            var totalPaymentAmount = paymentAmount + fee;

            var senderCard = new Card
            {
                Id = Guid.NewGuid(),
                Number = senderNumber,
                Balance = 50m,
                CreditLimit = 0m,
                Status = CardStatus.Active
            };

            var recipientCard = new Card
            {
                Id = Guid.NewGuid(),
                Number = recipientNumber,
                Balance = 50m,
                CreditLimit = null,
                Status = CardStatus.Active
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<AuthorizeCardCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _cardRepositoryMock
                .Setup(repo => repo.GetSingleAsync(It.IsAny<ActiveCardSpecification>()))!
                .ReturnsAsync((ActiveCardSpecification spec) =>
                {
                    return spec.Criteria.Compile()(senderCard) ? senderCard :
                           spec.Criteria.Compile()(recipientCard) ? recipientCard : null;
                });

            var feeRecord = new Fee
            {
                Id = Guid.NewGuid(),
                Amount = feeMultiplier,
                Timestamp = DateTime.UtcNow
            };
            _feeRepositoryMock
                .Setup(repo => repo.GetSingleAsync(It.IsAny<LatestFeeSpecification>()))
                .ReturnsAsync(feeRecord);

            var command = new PayWithCardCommand(senderNumber, recipientNumber, paymentAmount);

            // Act & Assert
            await Assert.ThrowsAsync<InsufficientFundsException>(() => _handler.Handle(command, CancellationToken.None));

            _transactionFactoryMock.Verify(tf => tf.Create(It.IsAny<Card>(), It.IsAny<Card>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }
    }
}
