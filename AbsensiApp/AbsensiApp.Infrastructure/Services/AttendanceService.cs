using AbsensiApp.Application.DTOs;
using AbsensiApp.Application.Interfaces;
using AbsensiApp.Domain.Entities;

namespace AbsensiApp.Infrastructure.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IQrCodeRepository _qrCodeRepository;
        private readonly IOfficeLocationRepository _officeLocationRepository;

        public AttendanceService(
            IAttendanceRepository attendanceRepository,
            IQrCodeRepository qrCodeRepository,
            IOfficeLocationRepository officeLocationRepository)
        {
            _attendanceRepository = attendanceRepository;
            _qrCodeRepository = qrCodeRepository;
            _officeLocationRepository = officeLocationRepository;
        }

        public async Task<QrCodeResponseDto> GenerateQrCodeAsync()
        {
            await _qrCodeRepository.DeleteExpiredAsync();

            var qrCode = new QrCode
            {
                Id = Guid.NewGuid(),
                Code = Guid.NewGuid().ToString("N").ToUpper(),
                Date = DateTime.UtcNow.Date,
                ExpiredAt = DateTime.UtcNow.AddHours(8),
                CreatedAt = DateTime.UtcNow
            };

            await _qrCodeRepository.CreateAsync(qrCode);

            return new QrCodeResponseDto
            {
                Id = qrCode.Id,
                Code = qrCode.Code,
                Date = qrCode.Date,
                ExpiredAt = qrCode.ExpiredAt
            };
        }

        public async Task<AttendanceResponseDto> CheckInQrAsync(Guid employeeId, CheckInQrRequestDto request)
        {
            var qrCode = await _qrCodeRepository.GetValidCodeAsync(request.QrCode);
            if (qrCode == null)
                throw new InvalidOperationException("QR Code tidak valid atau sudah expired!");

            return await CreateAttendanceAsync(employeeId, "QR", null, null);
        }

        public async Task<AttendanceResponseDto> CheckInGpsAsync(Guid employeeId, CheckInGpsRequestDto request)
        {
            var officeLocation = await _officeLocationRepository.GetActiveAsync();
            if (officeLocation == null)
                throw new InvalidOperationException("Lokasi kantor belum diatur!");

            var distance = CalculateDistance(
                request.Latitude, request.Longitude,
                officeLocation.Latitude, officeLocation.Longitude);

            if (distance > officeLocation.RadiusInMeters)
                throw new InvalidOperationException($"Kamu berada {distance:F0}m dari kantor. Maksimal {officeLocation.RadiusInMeters}m!");

            return await CreateAttendanceAsync(employeeId, "GPS", request.Latitude, request.Longitude);
        }

        private async Task<AttendanceResponseDto> CreateAttendanceAsync(
            Guid employeeId, string method, double? lat, double? lng)
        {
            var today = DateTime.UtcNow.Date;
            var existing = await _attendanceRepository.GetByEmployeeAndDateAsync(employeeId, today);

            if (existing != null)
                throw new InvalidOperationException("Kamu sudah absen masuk hari ini!");

            var now = DateTime.UtcNow;
            var status = now.Hour >= 9 ? "Late" : "Present";

            var attendance = new Attendance
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId,
                Date = today,
                CheckIn = now,
                CheckInMethod = method,
                CheckInLatitude = lat,
                CheckInLongitude = lng,
                Status = status,
                CreatedAt = now
            };

            await _attendanceRepository.CreateAsync(attendance);

            var result = await _attendanceRepository.GetByEmployeeAndDateAsync(employeeId, today);

            return new AttendanceResponseDto
            {
                Id = result!.Id,
                EmployeeName = result.Employee.FullName,
                EmployeeCode = result.Employee.EmployeeCode,
                Date = result.Date,
                CheckIn = result.CheckIn,
                CheckInMethod = result.CheckInMethod,
                Status = result.Status
            };
        }

        public async Task<AttendanceResponseDto> CheckOutAsync(Guid employeeId, CheckOutRequestDto request)
        {
            var today = DateTime.UtcNow.Date;
            var attendance = await _attendanceRepository.GetByEmployeeAndDateAsync(employeeId, today);

            if (attendance == null)
                throw new InvalidOperationException("Kamu belum absen masuk hari ini!");

            if (attendance.CheckOut != null)
                throw new InvalidOperationException("Kamu sudah absen keluar hari ini!");

            attendance.CheckOut = DateTime.UtcNow;
            attendance.CheckOutMethod = request.Latitude != null ? "GPS" : "Manual";
            attendance.CheckOutLatitude = request.Latitude;
            attendance.CheckOutLongitude = request.Longitude;

            await _attendanceRepository.UpdateAsync(attendance);

            return new AttendanceResponseDto
            {
                Id = attendance.Id,
                EmployeeName = attendance.Employee.FullName,
                EmployeeCode = attendance.Employee.EmployeeCode,
                Date = attendance.Date,
                CheckIn = attendance.CheckIn,
                CheckOut = attendance.CheckOut,
                CheckInMethod = attendance.CheckInMethod,
                CheckOutMethod = attendance.CheckOutMethod,
                Status = attendance.Status
            };
        }

        public async Task<List<AttendanceResponseDto>> GetMyAttendanceAsync(Guid employeeId)
        {
            var attendances = await _attendanceRepository.GetByEmployeeIdAsync(employeeId);
            return attendances.Select(a => new AttendanceResponseDto
            {
                Id = a.Id,
                EmployeeName = a.Employee.FullName,
                EmployeeCode = a.Employee.EmployeeCode,
                Date = a.Date,
                CheckIn = a.CheckIn,
                CheckOut = a.CheckOut,
                CheckInMethod = a.CheckInMethod,
                CheckOutMethod = a.CheckOutMethod,
                Status = a.Status,
                Notes = a.Notes
            }).ToList();
        }

        public async Task<List<AttendanceResponseDto>> GetAllAttendanceAsync(DateTime? date)
        {
            var attendances = await _attendanceRepository.GetAllByDateAsync(date);
            return attendances.Select(a => new AttendanceResponseDto
            {
                Id = a.Id,
                EmployeeName = a.Employee.FullName,
                EmployeeCode = a.Employee.EmployeeCode,
                Date = a.Date,
                CheckIn = a.CheckIn,
                CheckOut = a.CheckOut,
                CheckInMethod = a.CheckInMethod,
                CheckOutMethod = a.CheckOutMethod,
                Status = a.Status,
                Notes = a.Notes
            }).ToList();
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371000;
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
    }
}