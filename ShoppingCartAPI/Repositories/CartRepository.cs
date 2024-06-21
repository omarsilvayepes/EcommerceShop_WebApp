using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingCartAPI.Data;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Models.Dto;
using ShoppingCartAPI.Models.Dtos;
using ShoppingCartAPI.Services.IServices;

namespace ShoppingCartAPI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public CartRepository(AppDbContext appDbContext, IMapper mapper, IProductService productService)
        {
            _productService = productService;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }
        public async Task<CartDto> CartUpsert(CartDto cartDto)
        {
            var cartHeaderRecord = await _appDbContext.cartHeaders.AsNoTracking()
                .FirstOrDefaultAsync(u=> u.UserId==cartDto.CartHeader.UserId);
            if (cartHeaderRecord == null)
            {
                //Create Header  and Details

                CartHeader cartHeader=_mapper.Map<CartHeader>(cartDto.CartHeader);
                _appDbContext.Add(cartHeader);
                await _appDbContext.SaveChangesAsync();

                cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                _appDbContext.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                //check if details have the same product
                var cartDetailRecord = await _appDbContext.cartDetails.AsNoTracking()
                    .FirstOrDefaultAsync(
                     c=> c.ProductId==cartDto.CartDetails.First().ProductId &&
                     c.CartHeaderId==cartHeaderRecord.CartHeaderId
                    );
                if (cartDetailRecord == null)
                {
                    //Create new details

                    cartDto.CartDetails.First().CartHeaderId = cartHeaderRecord.CartHeaderId;
                    _appDbContext.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _appDbContext.SaveChangesAsync();
                }
                else
                {
                    //update count in cart details

                    cartDto.CartDetails.First().Count += cartDetailRecord.Count;
                    cartDto.CartDetails.First().CartHeaderId = cartDetailRecord.CartHeaderId;
                    cartDto.CartDetails.First().CartDetailsId = cartDetailRecord.CartDetailsId;
                    _appDbContext.cartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _appDbContext.SaveChangesAsync();
                }

            }
            return cartDto;
        }

        public async Task<CartDto> GetCart(string userId)
        {
            //TODO: Use  Linq for join querys

            CartDto cartDto = new()
            {
                CartHeader = _mapper.Map<CartHeaderDto>(_appDbContext.cartHeaders.First(u=> u.UserId==userId))
            };

            cartDto.CartDetails=_mapper.Map<IEnumerable<CartDetailsDto>>(_appDbContext.cartDetails
                .Where(c=> c.CartHeaderId==cartDto.CartHeader.CartHeaderId));

            //Call the Api microservice Product

            IEnumerable<ProductDto> productDtos = await _productService.GetProductsAsync();

            foreach(var item in cartDto.CartDetails)
            {
                item.Product=productDtos.FirstOrDefault(p=> p.ProductId==item.ProductId);
                cartDto.CartHeader.CarTotal += (item.Count * item.Product.Price);
            }

            return cartDto;
        }

        public async Task RemoveCart(int CardetailsId)
        {
            //TODO: Use  Linq for join querys

            CartDetails cartDetails = _appDbContext.cartDetails
                .First(c => c.CartDetailsId == CardetailsId);

            int totalCountOfCartItem = _appDbContext.cartDetails
                .Where(c => c.CartHeaderId == cartDetails.CartHeaderId)
                .Count();

            _appDbContext.Remove(cartDetails);

            if (totalCountOfCartItem == 1)
            {
                var carHeaderToRemove =await _appDbContext.cartHeaders
                    .FirstOrDefaultAsync(c => c.CartHeaderId == cartDetails.CartHeaderId);

                _appDbContext.Remove(carHeaderToRemove);
            }

            await _appDbContext.SaveChangesAsync();
        }
    }
}
