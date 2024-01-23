using CouponWeb.Models;

namespace CouponWeb.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto);
        Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto);
        Task<ResponseDto?> GetAllCouponAsync();
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> GetCouponByCodeAsync(string code);
        Task<ResponseDto?> DeleteCouponAsync(int id);
    }
}
