using GoodsAndOrders.Model.Entities;
using GoodsAndOrders.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoodsAndOrders.Model.ModelApi;
using GoodsAndOrders.Common;

namespace GoodsAndOrders.Controllers
{
    [Authorize]
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(int page = 1, int pageSize = 10, string? search = null, decimal? minPrice = null, decimal? maxPrice = null, Guid? categoryId = null)
        {
            var result = await _productService.GetProductsAsync(page, pageSize, search, minPrice, maxPrice, categoryId);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });

            return Ok(result.Value);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromBody] UpdateProductModelApi updateProductModelApi)
        {
            var result = await _productService.CreateProductAsync(updateProductModelApi);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });

            return Ok(result.Value);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductModelApi updateProductModelApi)
        {
            var result = await _productService.UpdateProductAsync(id, updateProductModelApi);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });

            return Ok(result.Value);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await _productService.DeleteProductAsync(id);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });

            return Ok(result.Value);
        }
    }
}
