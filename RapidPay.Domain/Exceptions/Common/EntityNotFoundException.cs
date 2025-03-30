namespace RapidPay.Domain.Exceptions.Common
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(Type type) : base($"Entity is not found (entity type: {type.Name}.)") { }
        public EntityNotFoundException(Type type, Guid id) : base($"Entity is not found (entity type: {type.Name}, id: {id}.)") { }
    }

    public class EntityNotFoundException<T> : EntityNotFoundException
        where T : class
    {
        public EntityNotFoundException() : base(typeof(T)) { }
        public EntityNotFoundException(Guid id) : base(typeof(T), id) { }
    }
}
