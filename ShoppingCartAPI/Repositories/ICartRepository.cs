using ShoppingCartAPI.Models.Dtos;

namespace ShoppingCartAPI.Repositories
{
    public interface ICartRepository
    {
        Task<CartDto> CartUpsert(CartDto cartDto);
        Task RemoveCart(int CardetailsId);
        Task ApplyCoupon(CartDto cartDto);
        Task RemoveCoupon(CartDto cartDto);
        Task<CartDto> GetCart(string userId);
        Task SendEmailShoppingCart(CartDto cartDto);

    }
}
