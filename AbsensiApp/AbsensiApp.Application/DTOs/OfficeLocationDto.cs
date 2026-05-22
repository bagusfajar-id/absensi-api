namespace AbsensiApp.Application.DTOs
{
    public class OfficeLocationRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double RadiusInMeters { get; set; }
    }

    public class OfficeLocationResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double RadiusInMeters { get; set; }
        public bool IsActive { get; set; }
    }
}