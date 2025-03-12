using GoodsAndOrders.Model.Entities;
using GoodsAndOrders.Services;
using GoodsAndOrders.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodsAndOrders.Controllers
{
    [Authorize]
    [Route("api/orderstatus")]
    [ApiController]
    public class OrderStatusController : ControllerBase
    {
        private readonly OrderStatusService _orderStatusService;

        public OrderStatusController(OrderStatusService orderStatusService)
        {
            _orderStatusService = orderStatusService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderStatus>>> GetAllStatuses()
        {
            var result = await _orderStatusService.GetAllStatusesAsync();

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });

            return Ok(result.Value);
        }
    }
}
