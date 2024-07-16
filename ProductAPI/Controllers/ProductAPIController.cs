using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models.Dto;
using ProductAPI.Repositories;

namespace CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private ResponseDto _responseDto;

        public ProductAPIController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _responseDto=new ResponseDto();
        }

        [HttpGet]
        public async Task<ResponseDto> Get()
        {
            IEnumerable<ProductDto> productsList;
            try
            {
                productsList= await _productRepository.GetProducts();
                _responseDto.Result = productsList;
                _responseDto.Message = "Products Got Sucessfully.";
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess=false;
                _responseDto.Message=ex.Message;
            }
            return _responseDto;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ResponseDto> Get(int id)
        {
            ProductDto product;
            try
            {
                product = await _productRepository.GetProductById(id);
                _responseDto.Result = product;
                _responseDto.Message = "Product Got Sucessfully.";
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }


        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ResponseDto> Post([FromBody] ProductDto productDto)
        {
            ProductDto product;
            try
            {
                product = await _productRepository.CreateProduct(productDto);
                _responseDto.Result = product;
                _responseDto.Message = "Product Created Sucessfully.";
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ResponseDto> Put([FromBody] ProductDto productDto)
        {
            ProductDto product;
            try
            {
                product = await _productRepository.UpdateProduct(productDto);
                _responseDto.Result = product;
                _responseDto.Message = "Product Updated Sucessfully.";
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ResponseDto> Delete(int id)
        {
            ProductDto productDto;
            try
            {
                await _productRepository.DeleteProduct(id);
                _responseDto.Message = "Product Deleted Sucessfully.";
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

    }
}
