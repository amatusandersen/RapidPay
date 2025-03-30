using MediatR;
using RapidPay.Application.Dtos;
using RapidPay.Application.Mappings;
using RapidPay.Domain.Interfaces.Factories;
using RapidPay.Domain.Interfaces.Infrastructure;

namespace RapidPay.Application.UseCases.Commands.CreateCard
{
    public class CreateCardCommandHandler(IUnitOfWork unitOfWork, ICardFactory cardFactory) : IRequestHandler<CreateCardCommand, CardModel>
    {
        public async Task<CardModel> Handle(CreateCardCommand request, CancellationToken cancellationToken)
        {
            var card = cardFactory.Create(request.InitialBalance, request.CreditLimit);

            await unitOfWork.CardRepository.AddAsync(card);
            await unitOfWork.SaveChangesAsync();

            return card.ToDto();
        }
    }
}
