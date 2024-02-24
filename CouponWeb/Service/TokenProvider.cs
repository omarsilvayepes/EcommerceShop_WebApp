using CouponWeb.Service.IService;
using CouponWeb.Utility;
using Newtonsoft.Json.Linq;

namespace CouponWeb.Service
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        void ITokenProvider.ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(SD.TokenCookie);
        }

        string? ITokenProvider.GetToken()
        {
            string? token= null;
            bool? hasToken = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(SD.TokenCookie,out token);

            return hasToken is true ? token : null;
        }

        void ITokenProvider.SetToken(string token)
        {
            _contextAccessor.HttpContext?.Response.Cookies.Append(SD.TokenCookie,token);
        }
    }
}
