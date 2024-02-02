using CouponWeb.Models;
using CouponWeb.Service.IService;
using CouponWeb.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace CouponWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            ResponseDto result = await _authService.LoginAsync(loginRequestDto);

            if (result != null && result.IsSuccess)
            {

                LoginResponseDto loginResponseDto = JsonConvert
                    .DeserializeObject<LoginResponseDto>(Convert.ToString(result.Result));

                return RedirectToAction("Index","Home");
            }
            else
            {
                ModelState.AddModelError("CustomError",result.Message);
                return View(loginRequestDto);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
                new SelectListItem{Text=SD.RoleCustomer,Value=SD.RoleCustomer}
            };

            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestDto)
        {
            ResponseDto result=await _authService.RegisterAsync(registrationRequestDto);

            if(result!=null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(registrationRequestDto.Role))
                {
                    //Add customer role by default

                    registrationRequestDto.Role = SD.RoleCustomer;
                }

                ResponseDto assingRole = await _authService.AssingRoleAsync(registrationRequestDto);

                if(assingRole!=null && assingRole.IsSuccess)
                {
                    TempData["success"] = "Registration Sucessfull";
                    return RedirectToAction(nameof(Login));
                }

            }

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
                new SelectListItem{Text=SD.RoleCustomer,Value=SD.RoleCustomer}
            };

            ViewBag.RoleList = roleList;
            return View(registrationRequestDto);
        }


        public IActionResult Logout()
        {
            return View();
        }
    }
}
