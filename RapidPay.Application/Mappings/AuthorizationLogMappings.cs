using RapidPay.Application.Dtos;
using RapidPay.Domain.Entities;

namespace RapidPay.Application.Mappings
{
    public static class AuthorizationLogMappings
    {
        public static AuthorizationLogModel ToDto(this AuthorizationLog entity)
        {
            if (entity == null)
            {
                return null!;
            }

            var model = new AuthorizationLogModel
            {
                Id = entity.Id,
                CardId = entity.CardId,
                Timestamp = entity.Timestamp,
                IsAuthorized = entity.IsAuthorized,
                Reason = entity.Reason
            };

            return model;
        }

        public static AuthorizationLog ToEntity(this AuthorizationLogModel model)
        {
            if (model == null)
            {
                return null!;
            }

            var entity = new AuthorizationLog
            {
                Id = model.Id,
                CardId = model.CardId,
                Timestamp = model.Timestamp,
                IsAuthorized = model.IsAuthorized,
                Reason = model.Reason
            };

            return entity;
        }
    }
}
