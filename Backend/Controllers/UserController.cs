using GoodsAndOrders.Model.Entities;
using GoodsAndOrders.Services;
using Microsoft.AspNetCore.Mvc;
using GoodsAndOrders.Model.ModelApi;
using Microsoft.AspNetCore.Authorization;
using GoodsAndOrders.Common;
using System.Linq;

namespace GoodsAndOrders.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("dto")]
        public async Task<IActionResult> GetAllUsersForEdit(int page = 1, int pageSize = 10, string? search = null, Guid? userRoleId = null)
        {
            var result = await _userService.GetAllUsersForEditAsync(page, pageSize, search, userRoleId);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserModelApi userModelApi, [FromQuery] string password)
        {
            var result = await _userService.CreateUserAsync(userModelApi, password);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });

            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<IActionResult> CreateUserOnRegister([FromBody] UserModelApi userModelApi, [FromQuery] string password)
        {
            var result = await _userService.CreateUserOnRegistrationAsync(userModelApi, password);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });

            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserModelApi updateUserModelApi)
        {
            var result = await _userService.UpdateUserAsync(id, updateUserModelApi);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var userId = new Guid(User.Claims.FirstOrDefault(c => c.Type == "guid").Value);

            var result = await _userService.DeleteUserAsync(id, userId);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });

            return Ok(result.Value);
        }
    }
}
