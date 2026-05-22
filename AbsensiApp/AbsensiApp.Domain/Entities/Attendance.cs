namespace AbsensiApp.Domain.Entities
{
    public class Attendance
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        public DateTime Date { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string CheckInMethod { get; set; } = string.Empty; // QR / GPS
        public string? CheckOutMethod { get; set; }
        public double? CheckInLatitude { get; set; }
        public double? CheckInLongitude { get; set; }
        public double? CheckOutLatitude { get; set; }
        public double? CheckOutLongitude { get; set; }
        public string Status { get; set; } = string.Empty; // Present, Late, Absent
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}