using MediatR;
using RapidPay.Domain.Entities;
using RapidPay.Domain.Exceptions.AuthorizationLogs;
using RapidPay.Domain.Exceptions.Common;
using RapidPay.Domain.Interfaces.Infrastructure;
using RapidPay.Infrastructure.Persistence.Specifications.AuthorizationLogs;
using RapidPay.Infrastructure.Persistence.Specifications.Cards;

namespace RapidPay.Application.UseCases.Commands.AuthorizeCard
{
    public class AuthorizeCardCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AuthorizeCardCommand, bool>
    {
        public async Task<bool> Handle(AuthorizeCardCommand request, CancellationToken cancellationToken)
        {
            var activeCardSpec = new ActiveCardSpecification(request.CardNumber);
            var card = await unitOfWork.CardRepository.GetSingleAsync(activeCardSpec)
                ?? throw new EntityNotFoundException<Card>();

            var duplicateLogSpec = new DuplicateAuthorizationSpecification(card.Id);
            var latestLog = await unitOfWork.AuthorizationLogRepository.GetSingleAsync(duplicateLogSpec);
            
            // check to avoid duplicate transactions
            if (latestLog != null && (DateTime.UtcNow - latestLog.Timestamp).TotalSeconds < 5)
            {
                var exception = new DuplicateAuthorizationAttemptException(card.Id, DateTime.UtcNow);
                await unitOfWork.AuthorizationLogRepository.LogAuthorization(card.Id, false, exception.Message);
                await unitOfWork.SaveChangesAsync();

                throw exception;
            }

            await unitOfWork.AuthorizationLogRepository.LogAuthorization(card.Id, true, string.Empty);
            await unitOfWork.SaveChangesAsync();
            
            return true;
        }
    }
}
