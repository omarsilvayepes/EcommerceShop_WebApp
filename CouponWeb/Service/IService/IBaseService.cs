using CouponWeb.Models;

namespace CouponWeb.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto,bool withBearer=true);
    }
}
