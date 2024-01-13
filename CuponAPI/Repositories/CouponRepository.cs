using CouponAPI.Data;
using CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CouponAPI.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly AppDbContext _appDbContext;

        public CouponRepository(AppDbContext appDbContext)
        {
            _appDbContext=appDbContext;
        }

        public async Task<Coupon> GetCuponById(int id)
        {
            return await _appDbContext.Coupons.FirstAsync(c=> c.CuponId.Equals(id));
        }

        public async Task<IEnumerable<Coupon>> GetCupons()
        {

            return await _appDbContext.Coupons.ToListAsync();
        }
    }
}
