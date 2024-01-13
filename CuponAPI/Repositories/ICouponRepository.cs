using CouponAPI.Models;

namespace CouponAPI.Repositories
{
    public interface ICouponRepository
    {
        Task<IEnumerable<Coupon>> GetCupons();
        Task<Coupon> GetCuponById(int id);
    }
}
