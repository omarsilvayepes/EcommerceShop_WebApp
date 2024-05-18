using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;
using ProductAPI.Models.Dto;
using ProductPI.Data;

namespace ProductAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public ProductRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _mapper = mapper;
            _appDbContext=appDbContext;
        }

        public async Task<ProductDto> CreateProduct(ProductDto productDto)
        {
            Product product=_mapper.Map<Product>(productDto);
            _appDbContext.Products.Add(product);
            await _appDbContext.SaveChangesAsync();
            return productDto;
        }

        public async Task DeleteProduct(int id)
        {
            Product product = await _appDbContext.Products.FirstAsync(p => p.ProductId.Equals(id));
            _appDbContext.Products.Remove(product);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            Product product=await _appDbContext.Products.FirstAsync(p=> p.ProductId.Equals(id));
            return _mapper.Map<ProductDto>(product);

        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            IEnumerable<Product> products =await _appDbContext.Products.ToListAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> UpdateProduct(ProductDto productDto)
        {
            Product product = _mapper.Map<Product>(productDto);
            _appDbContext.Products.Update(product);
            await _appDbContext.SaveChangesAsync();
            return productDto;
        }
    }
}
