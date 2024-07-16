using CouponAPI.Models.Dto;
using CouponAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            IEnumerable<CouponDto> cuponList;
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
            CouponDto cupon;
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

        [HttpGet]
        [Route("GetByCode/{code}")]
        public async Task<ResponseDto> GetByCode(string code)
        {
            CouponDto cupon;
            try
            {
                cupon = await _couponRepository.GetCuponByCode(code);
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


        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ResponseDto> Post([FromBody] CouponDto couponDto)
        {
            CouponDto cupon;
            try
            {
                cupon = await _couponRepository.CreateCoupon(couponDto);
                _responseDto.Result = cupon;
                _responseDto.Message = "Coupon Created Sucessfully.";
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ResponseDto> Put([FromBody] CouponDto couponDto)
        {
            CouponDto cupon;
            try
            {
                cupon = await _couponRepository.UpdateCoupon(couponDto);
                _responseDto.Result = cupon;
                _responseDto.Message = "Coupon Updated Sucessfully.";
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ResponseDto> Delete(int id)
        {
            CouponDto cupon;
            try
            {
                await _couponRepository.DeleteCoupon(id);
                _responseDto.Message = "Coupon Deleted Sucessfully.";
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
