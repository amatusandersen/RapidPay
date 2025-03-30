using RapidPay.Domain.Entities;
using RapidPay.Domain.Interfaces.Factories;
using System.Text.Json;

namespace RapidPay.Application.Services.Factories
{
    public class ManualCardUpdateFactory : IManualCardUpdateFactory
    {
        public ManualCardUpdate Create(Guid cardId, List<string> updatedFields, Dictionary<string, string> oldValues, Dictionary<string, string> newValues)
        {
            var entity = new ManualCardUpdate
            {
                Id = Guid.NewGuid(),
                CardId = cardId,
                UpdatedFields = string.Join(", ", updatedFields),
                OldValues = JsonSerializer.Serialize(oldValues),
                NewValues = JsonSerializer.Serialize(newValues),
            };

            return entity;
        }
    }
}
