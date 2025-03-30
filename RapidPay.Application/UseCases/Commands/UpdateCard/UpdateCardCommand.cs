using MediatR;
using RapidPay.Application.Dtos;
using RapidPay.Domain.Enums;

namespace RapidPay.Application.UseCases.Commands.UpdateCard
{
    public class UpdateCardCommand(string cardNumber, decimal? newBalance, decimal? newCreditLimit, CardStatus? newCardStatus) : IRequest<CardModel>
    {
        public string CardNumber { get; } = cardNumber;
        public decimal? NewBalance { get; } = newBalance;
        public decimal? NewCreditLimit { get; } = newCreditLimit;
        public CardStatus? NewCardStatus { get; } = newCardStatus;
    }
}
