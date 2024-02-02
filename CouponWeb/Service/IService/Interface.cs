using CouponWeb.Models;

namespace CouponWeb.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?> AssingRoleAsync(RegistrationRequestDto registrationRequestDto);
    }
}
