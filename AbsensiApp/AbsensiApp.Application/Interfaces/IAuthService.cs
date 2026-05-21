using AbsensiApp.Application.DTOs;

namespace AbsensiApp.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}