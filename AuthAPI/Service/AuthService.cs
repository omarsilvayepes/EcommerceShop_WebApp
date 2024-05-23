using AuthAPI.Data;
using AuthAPI.Models;
using AuthAPI.Models.Dto;
using AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _Dbcontext;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly IJwtTokenGenerator _JwtTokenGenerator;
        public AuthService(
            RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUser> userManager,
            AppDbContext appDbContext,
            IJwtTokenGenerator jwtTokenGenerator
            )
        {
            _Dbcontext = appDbContext;
            _UserManager = userManager;
            _RoleManager = roleManager;
            _JwtTokenGenerator = jwtTokenGenerator;

        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var userDB = _Dbcontext.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if(userDB != null )
            {
                if(!_RoleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult()) 
                {
                    //Create Role if does not exist
                    _RoleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _UserManager.AddToRoleAsync(userDB,roleName); // Here it is posible use AddToRolesAsync for add multiple roles to user
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var userDB = await _Dbcontext.ApplicationUsers.FirstOrDefaultAsync(u=>u.UserName.ToLower()==loginRequestDto.UserName.ToLower());

            bool isValidUser = await _UserManager.CheckPasswordAsync(userDB, loginRequestDto.Password);

            if(!isValidUser || userDB is null)
            {
                return new (){ User=null, Token=string.Empty};
            }

            //if user was found generate Json web token and get User's roles
            var roles=await _UserManager.GetRolesAsync(userDB);
            var token = _JwtTokenGenerator.GenerateToken(userDB,roles);

            UserDto userDto = new()
            {
                ID= userDB.Id,
                Email= userDB.Email,
                Name=userDB.Name,
                PhoneNumber=userDB.PhoneNumber
            };

            LoginResponseDto loginResponseDto = new()
            {
                User=userDto,
                Token=token
            };
            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name=registrationRequestDto.Name,
                PhoneNumber=registrationRequestDto.PhoneNumber
            };

            try
            {
                var result = await _UserManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    ApplicationUser? userToReturn = await _Dbcontext
                        .ApplicationUsers
                        .FirstOrDefaultAsync(user=>user.Email==registrationRequestDto.Email);

                    if (userToReturn!=null)
                    {
                        UserDto userDto = new()
                        {
                            ID = userToReturn.Id,
                            Email =userToReturn.Email,
                            Name=userToReturn.Name,
                            PhoneNumber=userToReturn.PhoneNumber
                        };

                        return string.Empty;
                    }
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
            }
            return "Error Encountered";
        }
    }
}
