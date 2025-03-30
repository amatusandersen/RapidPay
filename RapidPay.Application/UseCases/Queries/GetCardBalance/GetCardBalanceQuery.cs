using MediatR;
using RapidPay.Application.Dtos;

namespace RapidPay.Application.UseCases.Queries.GetCardBalance
{
    public class GetCardBalanceQuery(string cardNumber) : IRequest<CardBalanceModel>
    {
        public string CardNumber { get; } = cardNumber;
    }
}
