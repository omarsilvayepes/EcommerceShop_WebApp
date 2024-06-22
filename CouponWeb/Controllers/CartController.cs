using CouponWeb.Models;
using CouponWeb.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace CouponWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            //return View(await LoadCarDtoBaseOnLoginUser());
            return null;

            //TODO: have issue here!!!
        }

        private async Task<CartDto> LoadCarDtoBaseOnLoginUser()
        {
            var userId = User.Claims.Where(u=> u.Type==JwtRegisteredClaimNames.Sub)?
                .FirstOrDefault()?.Value;
            ResponseDto response=await cartService.GetCartByUserIdAsync(userId);

            if(response is not null && response.IsSuccess)
            {
               return JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
            }
            return new CartDto();
        }
    }
}
