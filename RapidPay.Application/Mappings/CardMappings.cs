using RapidPay.Application.Dtos;
using RapidPay.Domain.Entities;

namespace RapidPay.Application.Mappings
{
    public static class CardMappings
    {
        public static CardModel ToDto(this Card entity)
        {
            if (entity == null)
            {
                return null!;
            }

            var model = new CardModel
            {
                Id = entity.Id,
                Balance = entity.Balance,
                CreditLimit = entity.CreditLimit,
                Status = entity.Status,
                Number = entity.Number,
                CreatedAt = entity.CreatedAt
            };

            return model;
        }

        public static Card ToEntity(this CardModel model)
        {
            if (model == null)
            {
                return null!;
            }

            var entity = new Card
            {
                Id = model.Id,
                Balance = model.Balance,
                CreditLimit = model.CreditLimit,
                Status = model.Status,
                Number = model.Number,
                CreatedAt = model.CreatedAt
            };

            return entity;
        }
    }
}
