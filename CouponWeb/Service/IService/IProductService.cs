using CouponWeb.Models;

namespace CouponWeb.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto?> CreateProductAsync(ProductDto productDto);
        Task<ResponseDto?> UpdateProductAsync(ProductDto productDto);
        Task<ResponseDto?> GetAllProductAsync();
        Task<ResponseDto?> GetProductByIdAsync(int id);
        Task<ResponseDto?> DeleteProductAsync(int id);
    }
}
