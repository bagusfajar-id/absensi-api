namespace AbsensiApp.Domain.Entities
{
    public class QrCode
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime ExpiredAt { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}