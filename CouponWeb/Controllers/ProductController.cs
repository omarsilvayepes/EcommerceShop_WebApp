using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CouponWeb.Service.IService;
using CouponWeb.Models;

namespace ProductWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _ProductService;

        public ProductController(IProductService ProductService)
        {
            _ProductService = ProductService;
        }
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto>? list = new();

            ResponseDto? response = await _ProductService.GetAllProductAsync();

            if (response!=null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }

        public async Task<IActionResult> ProductCreate() // For Render the form  to the user
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreateController(ProductDto ProductDto) //Create Product :Receive Data from Form of view ProductCreate
        {
            if(ModelState.IsValid)
            {
                ResponseDto? response = await _ProductService.CreateProductAsync(ProductDto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = response?.Message;
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View("ProductCreate", ProductDto);
        }

        public async Task<IActionResult> ProductDelete(int ProductId) //For Render the View Product for Delete to the user 
		{
			ResponseDto? response = await _ProductService.GetProductByIdAsync(ProductId);

			if (response != null && response.IsSuccess)
			{
				ProductDto? ProductDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(ProductDto);
			}
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
		}


        [HttpPost]
        public async Task<IActionResult> ProductDeleteController(ProductDto ProductDto) //Delete Product :Receive Data from Form of view ProductDelete
        {
            ResponseDto? response = await _ProductService.DeleteProductAsync(ProductDto.ProductId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response?.Message;
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View("ProductDelete", ProductDto);
        }



        public async Task<IActionResult> ProductEdit(int ProductId) //For Render the View Product for Edit to the user 
        {
            ResponseDto? response = await _ProductService.GetProductByIdAsync(ProductId);

            if (response != null && response.IsSuccess)
            {
                ProductDto? ProductDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(ProductDto);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> ProductEditController(ProductDto ProductDto) //Edit Product :Receive Data from Form of view ProductEdit
        {
            ResponseDto? response = await _ProductService.UpdateProductAsync(ProductDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response?.Message;
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View("ProductEdit", ProductDto);
        }
    }
}
