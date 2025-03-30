namespace RapidPay.Application.Dtos
{
    public class AuthorizationLogModel
    {
        public Guid Id { get; set; }
        public Guid CardId { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsAuthorized { get; set; }
        public string Reason { get; set; }
    }
}
