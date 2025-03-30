namespace RapidPay.Domain.Exceptions.Users
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() : base("User already exists.") { }
        public UserAlreadyExistsException(string username) : base($"User already exists (username: {username}).") { }
    }
}
