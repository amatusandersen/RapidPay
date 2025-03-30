using MediatR;
using RapidPay.Application.Dtos;
using RapidPay.Application.Mappings;
using RapidPay.Domain.Entities;
using RapidPay.Domain.Exceptions.Common;
using RapidPay.Domain.Interfaces.Factories;
using RapidPay.Domain.Interfaces.Infrastructure;
using RapidPay.Infrastructure.Persistence.Specifications.Cards;

namespace RapidPay.Application.UseCases.Commands.Cards.UpdateCard
{
    public class UpdateCardCommandHandler(IUnitOfWork unitOfWork, IManualCardUpdateFactory updateFactory) : IRequestHandler<UpdateCardCommand, CardModel>
    {
        public async Task<CardModel> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
        {
            var cardSpecification = new GetCardByNumberSpecification(request.CardNumber);
            var card = await unitOfWork.CardRepository.GetSingleAsync(cardSpecification)
                ?? throw new EntityNotFoundException<Card>();

            var updatedFiels = new List<string>();
            var oldValues = new Dictionary<string, string>();
            var newValues = new Dictionary<string, string>();

            if (request.NewBalance != null && request.NewBalance.HasValue)
            {
                updatedFiels.Add(nameof(Card.Balance));
                oldValues[nameof(Card.Balance)] = card.Balance.ToString();
                newValues[nameof(Card.Balance)] = request.NewBalance.Value.ToString();
                card.Balance = request.NewBalance.Value;
            }

            if (request.NewCreditLimit.HasValue && request.NewCreditLimit.Value != (card.CreditLimit ?? 0))
            {
                updatedFiels.Add(nameof(Card.CreditLimit));
                oldValues[nameof(Card.CreditLimit)] = card.CreditLimit.ToString() ?? string.Empty;
                newValues[nameof(Card.CreditLimit)] = request.NewCreditLimit.ToString() ?? string.Empty;
                card.CreditLimit = request.NewCreditLimit;
            }

            if (request.NewCardStatus.HasValue && request.NewCardStatus.Value != Domain.Enums.CardStatus.Active)
            {
                updatedFiels.Add(nameof(Card.Status));
                oldValues[nameof(Card.Status)] = card.Status.ToString();
                newValues[nameof(Card.Status)] = request.NewCardStatus.Value.ToString();
                card.Status = request.NewCardStatus.Value;
            }

            if (updatedFiels.Count > 0)
            {
                var update = updateFactory.Create(card.Id, updatedFiels, oldValues, newValues);

                await unitOfWork.ManualCardUpdateRepository.AddAsync(update);
                await unitOfWork.SaveChangesAsync();

            }

            await unitOfWork.CardRepository.UpdateAsync(card);
            await unitOfWork.SaveChangesAsync();

            return card.ToDto();
        }
    }
}
