using GoodsAndOrders.Common;
using GoodsAndOrders.Model.Entities;
using GoodsAndOrders.Model.ModelApi;
using GoodsAndOrders.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoodsAndOrders.Controllers
{
    [Authorize]
    [Route("api/orders")]
    [ApiController]
    public class UserOrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public UserOrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] Guid? customerId, int page = 1, int pageSize = 10, Guid? orderStatusId = null)
        {
            var result = await _orderService.GetAllOrdersAsync(customerId, page, pageSize, orderStatusId);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderModelApi createOrderModelApi)
        {
            var result = await _orderService.CreateOrderAsync(createOrderModelApi);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });

            return Ok(result.Value);
        }

        [HttpPut("status")]
        [Authorize]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderModelApi updateOrderModelApi)
        {
            var result = await _orderService.UpdateOrderStatusAsync(updateOrderModelApi);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var result = await _orderService.DeleteOrderAsync(id);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });

            return Ok(result.Value);
        }
    }
}
