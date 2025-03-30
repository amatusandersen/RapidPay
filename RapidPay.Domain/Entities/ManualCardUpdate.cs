namespace RapidPay.Domain.Entities
{
    public class ManualCardUpdate
    {
        public Guid Id { get; set; }
        public Guid CardId { get; set; }
        public string UpdatedFields { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public DateTime Timestamp { get; set; }

        public Card Card { get; set; }
    }
}
