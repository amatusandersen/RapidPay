namespace RapidPay.Domain.Exceptions.Users
{
    public class UserIsNotRegisteredException : Exception
    {
        public UserIsNotRegisteredException() : base("User is not registered.") { }
        public UserIsNotRegisteredException(string username) : base($"User is not registered (username: {username}).") { }
    }
}
