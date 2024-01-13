using CouponAPI.Models;
using CouponAPI.Models.Dto;
using CouponAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;
        private ResponseDto _responseDto;

        public CouponAPIController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
            _responseDto=new ResponseDto();
        }

        [HttpGet]

        public async Task<ResponseDto> Get()
        {
            IEnumerable<Coupon> cuponList;
            try
            {
                cuponList= await _couponRepository.GetCupons();
                _responseDto.Result = cuponList;
                _responseDto.Message = "Coupons Got Sucessfully.";
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess=false;
                _responseDto.Message=ex.Message;
            }
            return _responseDto;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ResponseDto> Get(int id)
        {
            Coupon cupon;
            try
            {
                cupon = await _couponRepository.GetCuponById(id);
                _responseDto.Result = cupon;
                _responseDto.Message = "Coupon Got Sucessfully.";
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

    }
}
