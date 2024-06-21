using Microsoft.AspNetCore.Mvc;
using ShoppingCartAPI.Models.Dto;
using ShoppingCartAPI.Models.Dtos;
using ShoppingCartAPI.Repositories;

namespace ShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private ResponseDto _responseDto;

        public CartAPIController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
            _responseDto = new ResponseDto();
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            try
            {
                CartDto cart=await _cartRepository.CartUpsert(cartDto);
                _responseDto.Message = "Cart Created or Updated Sucessfully.";
                _responseDto.Result = cart;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartDto cart = await _cartRepository.GetCart(userId);
                _responseDto.Message = "Cart get it Sucessfully.";
                _responseDto.Result = cart;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody]int carDetailsId)
        {
            try
            {
                 await _cartRepository.RemoveCart(carDetailsId);
                _responseDto.Message = "Cart Deleted Sucessfully.";
                _responseDto.Result = true;
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
