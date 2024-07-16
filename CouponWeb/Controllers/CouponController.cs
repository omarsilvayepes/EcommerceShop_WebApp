using CouponWeb.Models;
using CouponWeb.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CouponWeb.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? list = new();

            ResponseDto? response = await _couponService.GetAllCouponAsync();

            if (response!=null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }

        public async Task<IActionResult> CouponCreate() // For Render the form  to the user
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreateController(CouponDto couponDto) //Create Coupon :Receive Data from Form of view CouponCreate
        {
            if(ModelState.IsValid)
            {
                ResponseDto? response = await _couponService.CreateCouponAsync(couponDto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = response?.Message;
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View("CouponCreate", couponDto);
        }

        public async Task<IActionResult> CouponDelete(int couponId) //For Render the View Coupon for Delete to the user 
		{
			ResponseDto? response = await _couponService.GetCouponByIdAsync(couponId);

			if (response != null && response.IsSuccess)
			{
				CouponDto? couponDto = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
                return View(couponDto);
			}
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
		}


        [HttpPost]
        public async Task<IActionResult> CouponDeleteController(CouponDto couponDto) //Delete Coupon :Receive Data from Form of view CouponDelete
        {
            ResponseDto? response = await _couponService.DeleteCouponAsync(couponDto.CuponId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response?.Message;
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View("CouponDelete", couponDto);
        }
    }
}
