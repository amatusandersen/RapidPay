using MediatR;
using RapidPay.Application.Dtos;
using RapidPay.Domain.Entities;
using RapidPay.Domain.Exceptions.Common;
using RapidPay.Domain.Interfaces.Infrastructure;
using RapidPay.Infrastructure.Persistence.Specifications.Cards;

namespace RapidPay.Application.UseCases.Queries.GetCardBalance
{
    public class GetCardBalanceQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCardBalanceQuery, CardBalanceModel>
    {
        public async Task<CardBalanceModel> Handle(GetCardBalanceQuery request, CancellationToken cancellationToken)
        {
            var cardSpecification = new GetCardByNumberSpecification(request.CardNumber);
            var card = await unitOfWork.CardRepository.GetSingleAsync(cardSpecification)
                ?? throw new EntityNotFoundException<Card>();

            return new CardBalanceModel
            {
                Balance = card.Balance,
                CreditLimit = card.CreditLimit
            };
        }
    }
}
