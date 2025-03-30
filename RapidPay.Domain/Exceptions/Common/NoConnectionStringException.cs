namespace RapidPay.Domain.Exceptions.Common
{
    public class NoConnectionStringException : Exception
    {
        public NoConnectionStringException() : base("No database connection string found.") { }
        public NoConnectionStringException(string connectionStringName) : base($"No database connection string found (connection string name: {connectionStringName}).") { }
    }
}
