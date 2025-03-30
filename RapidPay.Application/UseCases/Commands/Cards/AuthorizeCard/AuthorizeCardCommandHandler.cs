using MediatR;
using RapidPay.Domain.Entities;
using RapidPay.Domain.Exceptions.AuthorizationLogs;
using RapidPay.Domain.Exceptions.Common;
using RapidPay.Domain.Interfaces.Factories;
using RapidPay.Domain.Interfaces.Infrastructure;
using RapidPay.Infrastructure.Persistence.Specifications.AuthorizationLogs;
using RapidPay.Infrastructure.Persistence.Specifications.Cards;

namespace RapidPay.Application.UseCases.Commands.Cards.AuthorizeCard
{
    public class AuthorizeCardCommandHandler(IAuthorizationLogFactory logFactory, IUnitOfWork unitOfWork) : IRequestHandler<AuthorizeCardCommand, bool>
    {
        public async Task<bool> Handle(AuthorizeCardCommand request, CancellationToken cancellationToken)
        {
            var card = await GetActiveCardAsync(request.CardNumber);

            // check for dublicated transactions
            await EnsureNoDuplicateAuthorizationAsync(card);

            // log successful authorization
            await LogAuthorizationAsync(card.Id, true, string.Empty);

            return true;
        }

        private async Task<Card> GetActiveCardAsync(string cardNumber)
        {
            var spec = new ActiveCardSpecification(cardNumber);
            var card = await unitOfWork.CardRepository.GetSingleAsync(spec)
                       ?? throw new EntityNotFoundException<Card>();

            return card;
        }

        private async Task EnsureNoDuplicateAuthorizationAsync(Card card)
        {
            var spec = new DuplicateAuthorizationSpecification(card.Id);
            var latestLog = await unitOfWork.AuthorizationLogRepository.GetSingleAsync(spec);

            // If a duplicate attempt is detected within 5 seconds, log and throw an exception.
            if (latestLog != null && (DateTime.UtcNow - latestLog.Timestamp).TotalSeconds < 5)
            {
                var exception = new DuplicateAuthorizationAttemptException(card.Id, DateTime.UtcNow);

                await LogAuthorizationAsync(card.Id, false, exception.Message);

                throw exception;
            }
        }

        private async Task LogAuthorizationAsync(Guid cardId, bool isSuccess, string message)
        {
            var log = logFactory.Create(cardId, isSuccess, message);

            await unitOfWork.AuthorizationLogRepository.AddAsync(log);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
