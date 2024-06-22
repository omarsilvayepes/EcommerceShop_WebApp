using ShoppingCartAPI.Models.Dto;

namespace ShoppingCartAPI.Services.IServices
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string cuoponCode);
    }
}
