using GoodsAndOrders.Common;
using GoodsAndOrders.Model;
using GoodsAndOrders.UnitOfWork;
using GoodsAndOrders.Utils;
using Microsoft.EntityFrameworkCore;

namespace GoodsAndOrders.Services
{
    public class AuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtService _jwtService;

        public AuthService(IUnitOfWork unitOfWork, JwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        public async Task<Result<string?>> AuthenticateUserAsync(string login, string password)
        {
            var user = await _unitOfWork.Users.GetAll()
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.Login == login);

            if (user == null)
                return Result<string?>.Fail("Пользователь не найден", 404);

            if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
                return Result<string?>.Fail("Неверный пароль", 401);

            string token = _jwtService.GenerateToken(user.Id, user.UserRole?.Name ?? "User", user.Name);

            return Result<string?>.Success(token);
        }
    }

}
