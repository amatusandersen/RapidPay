using RapidPay.Domain.Entities;

namespace RapidPay.Domain.Interfaces.Factories
{
    public interface IManualCardUpdateFactory
    {
        ManualCardUpdate Create(Guid cardId, List<string> updatedFields, Dictionary<string, string> oldValues, Dictionary<string, string> newValues);
    }
}
