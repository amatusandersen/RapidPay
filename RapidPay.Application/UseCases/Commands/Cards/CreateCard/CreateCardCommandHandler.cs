using MediatR;
using RapidPay.Application.Dtos;
using RapidPay.Application.Mappings;
using RapidPay.Domain.Exceptions.Cards;
using RapidPay.Domain.Interfaces.Factories;
using RapidPay.Domain.Interfaces.Infrastructure;
using RapidPay.Infrastructure.Persistence.Specifications.Cards;

namespace RapidPay.Application.UseCases.Commands.Cards.CreateCard
{
    public class CreateCardCommandHandler(IUnitOfWork unitOfWork, ICardFactory cardFactory) : IRequestHandler<CreateCardCommand, CardModel>
    {
        public async Task<CardModel> Handle(CreateCardCommand request, CancellationToken cancellationToken)
        {
            var card = cardFactory.Create(request.InitialBalance, request.CreditLimit);

            var cardSpecification = new GetCardByNumberSpecification(card.Number);
            if (await unitOfWork.CardRepository.ExistsAsync(cardSpecification.Criteria))
            {
                throw new CardNumberAlreadyAssignedException(card.Number);
            }

            await unitOfWork.CardRepository.AddAsync(card);
            await unitOfWork.SaveChangesAsync();

            return card.ToDto();
        }
    }
}
