using MediatR;
using RapidPay.Application.UseCases.Commands.Cards.AuthorizeCard;
using RapidPay.Domain.Entities;
using RapidPay.Domain.Exceptions.Cards;
using RapidPay.Domain.Exceptions.Common;
using RapidPay.Domain.Interfaces.Factories;
using RapidPay.Domain.Interfaces.Infrastructure;
using RapidPay.Infrastructure.Persistence.Specifications.Cards;
using RapidPay.Infrastructure.Persistence.Specifications.Fees;

namespace RapidPay.Application.UseCases.Commands.Cards.PayWithCard
{
    public class PayWithCardCommandHandler(IMediator mediator, ITransactionFactory transactionFactory, IUnitOfWork unitOfWork) : IRequestHandler<PayWithCardCommand, bool>
    {
        private readonly object _lock = new();
        public async Task<bool> Handle(PayWithCardCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(new AuthorizeCardCommand(request.CardNumber), cancellationToken);

            var cardByNumberSpecification = new ActiveCardSpecification(request.CardNumber);
            var senderCard = await unitOfWork.CardRepository.GetSingleAsync(cardByNumberSpecification)
                ?? throw new EntityNotFoundException<Card>();

            var recipientCardSpecification = new ActiveCardSpecification(request.RecipientCardNumber);
            var recipientCard = await unitOfWork.CardRepository.GetSingleAsync(recipientCardSpecification)
                ?? throw new EntityNotFoundException<Card>();

            // calculate total payment considering current fee
            var latestFeeSpecification = new LatestFeeSpecification();
            var latestFee = await unitOfWork.FeeRepository.GetSingleAsync(latestFeeSpecification);

            var feeMultiplier = latestFee == null ? 0 : latestFee.Amount;
            var fee = request.PaymentAmount * feeMultiplier;
            var totalPaymentAmount = request.PaymentAmount + fee;

            lock (_lock)
            {
                // use credit funds if credit limit is not exceeded
                decimal additionalCredit = senderCard.CreditLimit ?? 0;
                decimal availableFunds = senderCard.Balance + additionalCredit;

                if (availableFunds < totalPaymentAmount)
                {
                    throw new InsufficientFundsException(senderCard.Id, request.PaymentAmount);
                }

                senderCard.Balance -= totalPaymentAmount;
                recipientCard.Balance += request.PaymentAmount;
            }

            var transaction = transactionFactory.Create(senderCard, recipientCard, totalPaymentAmount, fee);
            await unitOfWork.TransactionRepository.AddAsync(transaction);

            await unitOfWork.CardRepository.UpdateAsync(senderCard);
            await unitOfWork.CardRepository.UpdateAsync(recipientCard);

            await unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
