namespace RapidPay.Domain.Exceptions.Users
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException() : base("Invalid password.") { }
    }
}
