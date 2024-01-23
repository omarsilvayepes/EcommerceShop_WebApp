using CouponAPI.Models.Dto;

namespace CouponAPI.Repositories
{
    public interface ICouponRepository
    {
        Task<IEnumerable<CouponDto>> GetCupons();
        Task<CouponDto> GetCuponById(int id);
        Task<CouponDto> GetCuponByCode(string code);
        Task<CouponDto> CreateCoupon(CouponDto couponDto);
        Task<CouponDto> UpdateCoupon(CouponDto couponDto);
        Task DeleteCoupon(int id);
    }
}
