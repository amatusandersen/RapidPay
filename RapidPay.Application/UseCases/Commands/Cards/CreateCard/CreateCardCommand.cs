using MediatR;
using RapidPay.Application.Dtos;

namespace RapidPay.Application.UseCases.Commands.Cards.CreateCard
{
    public class CreateCardCommand(decimal initialBalance, decimal? creditLimit)
        : IRequest<CardModel>
    {
        public decimal InitialBalance { get; } = initialBalance;
        public decimal? CreditLimit { get; } = creditLimit;
    }
}
