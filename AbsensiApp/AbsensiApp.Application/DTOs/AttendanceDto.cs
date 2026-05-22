namespace AbsensiApp.Application.DTOs
{
    public class CheckInQrRequestDto
    {
        public string QrCode { get; set; } = string.Empty;
    }

    public class CheckInGpsRequestDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class CheckOutRequestDto
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

    public class AttendanceResponseDto
    {
        public Guid Id { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string CheckInMethod { get; set; } = string.Empty;
        public string? CheckOutMethod { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    public class QrCodeResponseDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}