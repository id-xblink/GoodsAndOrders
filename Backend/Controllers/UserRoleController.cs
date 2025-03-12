using GoodsAndOrders.Common;
using GoodsAndOrders.Model.ModelApi;
using GoodsAndOrders.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodsAndOrders.Controllers
{
    [Authorize]
    [Route("api/roles")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly UserRoleService _userRoleService;

        public UserRoleController(UserRoleService userService)
        {
            _userRoleService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userRoleService.GetAllUserRolesAsync();

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });

            return Ok(result.Value);
        }
    }
}
