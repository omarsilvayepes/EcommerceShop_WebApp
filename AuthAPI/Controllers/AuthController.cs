using AuthAPI.Models.Dto;
using AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _responseDto;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _responseDto = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var errorMessage=await _authService.Register(registrationRequestDto);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message=errorMessage;
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var logingResponse=await _authService.Login(loginRequestDto);
            if (logingResponse.User == null)
            {
                _responseDto.IsSuccess=false;
                _responseDto.Message = "User Name or Password is incorrect";
                return Unauthorized(_responseDto);
            }
            _responseDto.Result= logingResponse;
            return Ok(logingResponse);
        }

        [HttpPost("assingRole")]
        public async Task<IActionResult> AssingRole([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var isAssignRoleSuccess = await _authService.AssignRole(registrationRequestDto.Email,registrationRequestDto.Role.ToUpper());
            if (!isAssignRoleSuccess)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Can Not assign Role to the User";
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }
    }
}
