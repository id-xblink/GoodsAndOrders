using GoodsAndOrders.Model;
using GoodsAndOrders.Model.ModelApi;
using GoodsAndOrders.Services;
using GoodsAndOrders.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace GoodsAndOrders.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestApiModel logicRequest)
        {
            var result = await _authService.AuthenticateUserAsync(logicRequest.Login, logicRequest.Password);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });

            return Ok(new { token = result.Value });
        }
    }
}
