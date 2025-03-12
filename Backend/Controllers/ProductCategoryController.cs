using GoodsAndOrders.Model.ModelApi;
using GoodsAndOrders.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodsAndOrders.Controllers
{
    [Authorize]
    [Route("api/categories")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public ProductCategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _categoryService.GetAllCategoriesAsync();

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });

            return Ok(result.Value);
        }
    }
}
