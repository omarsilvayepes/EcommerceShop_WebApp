using CouponWeb.Models;
using CouponWeb.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace CouponWeb.Controllers
{
    public class CartController : Controller
    {

        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCarDtoBaseOnLoginUser());
        }

        private async Task<CartDto> LoadCarDtoBaseOnLoginUser()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?
                .FirstOrDefault()?.Value;
            ResponseDto response = await cartService.GetCartByUserIdAsync(userId);

            if (response is not null && response.IsSuccess && response.Result is not null )
            {
                return JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
            }
            return new CartDto();
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            ResponseDto? response = await cartService.RemoveFromCartAsync(cartDetailsId);

            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Remove item Sucessfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            ResponseDto? response = await cartService.ApplyCouponAsync(cartDto);

            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Apply Coupon Sucessfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {
            CartDto cart = await LoadCarDtoBaseOnLoginUser();
            cart.CartHeader.Email= User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?
                .FirstOrDefault()?.Value;

            ResponseDto? response = await cartService.EmailCart(cart);

            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Email will be process and send shortly.";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartHeader.CouponCode = string.Empty;
            ResponseDto? response = await cartService.ApplyCouponAsync(cartDto);

            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Remove Coupon Sucessfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

    }
}
