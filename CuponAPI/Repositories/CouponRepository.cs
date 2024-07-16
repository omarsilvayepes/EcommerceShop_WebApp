using AutoMapper;
using CouponAPI.Data;
using CouponAPI.Models;
using CouponAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace CouponAPI.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public CouponRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _mapper = mapper;
            _appDbContext=appDbContext;
        }

        public async Task<CouponDto> CreateCoupon(CouponDto couponDto)
        {
            Coupon coupon=_mapper.Map<Coupon>(couponDto);
            _appDbContext.Coupons.Add(coupon);
            await _appDbContext.SaveChangesAsync();
            return couponDto;
        }

        public async Task DeleteCoupon(int id)
        {
            Coupon coupon = await _appDbContext.Coupons.FirstAsync(c => c.CuponId.Equals(id));
            _appDbContext.Coupons.Remove(coupon);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<CouponDto> GetCuponByCode(string code)
        {
            Coupon coupon = await _appDbContext.Coupons.FirstAsync(c => c.CuponCode.ToLower()==code.ToLower());
            return _mapper.Map<CouponDto>(coupon);
        }

        public async Task<CouponDto> GetCuponById(int id)
        {
            Coupon coupon=await _appDbContext.Coupons.FirstAsync(c=> c.CuponId.Equals(id));
            return _mapper.Map<CouponDto>(coupon);

        }

        public async Task<IEnumerable<CouponDto>> GetCupons()
        {
            IEnumerable<Coupon> cupons =await _appDbContext.Coupons.ToListAsync();
            return _mapper.Map<IEnumerable<CouponDto>>(cupons);
        }

        public async Task<CouponDto> UpdateCoupon(CouponDto couponDto)
        {
            Coupon coupon = _mapper.Map<Coupon>(couponDto);
            _appDbContext.Coupons.Update(coupon);
            await _appDbContext.SaveChangesAsync();
            return couponDto;
        }
    }
}
