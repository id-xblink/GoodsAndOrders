using GoodsAndOrders.Model;
using GoodsAndOrders.Model.Entities;
using GoodsAndOrders.Model.ModelApi;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using GoodsAndOrders.Utils;
using GoodsAndOrders.UnitOfWork;
using GoodsAndOrders.Common;

namespace GoodsAndOrders.Services
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<object>> GetAllUsersForEditAsync(int page, int pageSize, string? search, Guid? userRoleId)
        {
            if (page < 1 || pageSize < 1)
                return Result<object>.Fail("Некорректные параметры пагинации", 400);

            var query = _unitOfWork.Users.GetAll().Include(u => u.UserRole).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Code.Contains(search));

            if (userRoleId.HasValue)
                query = query.Where(p => p.UserRoleId == userRoleId.Value);

            int totalItems = await query.CountAsync();

            var users = await query
                .OrderBy(u => u.Code)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserResponseModelApi
                {
                    Id = u.Id,
                    Login = u.Login,
                    Name = u.Name,
                    Code = u.Code,
                    Address = u.Address,
                    Discount = u.Discount,
                    UserRole = u.UserRole.Name
                })
                .ToListAsync();

            var data = new
            {
                users,
                totalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                totalItems
            };

            return Result<object>.Success(data);
        }

        public async Task<Result<UserModelApi>> CreateUserAsync(UserModelApi? userModelApi, string? password)
        {
            var validation = ValidateUser(userModelApi, password);
            if (!validation.Result.IsSuccess)
                return validation.Result;

            if (userModelApi.Discount < 0 || userModelApi.Discount > 100)
                return Result<UserModelApi>.Fail("Скидка должна быть в диапазоне 0-100", 422);

            var existingCodes = await _unitOfWork.Users.GetAll()
                .Select(u => u.Code)
                .ToListAsync();

            var newCode = UserCodeGenerator.GenerateUserCode(existingCodes);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = userModelApi.Name,
                Login = userModelApi.Login,
                Code = newCode,
                Address = userModelApi.Address,
                Discount = userModelApi.Discount,
                UserRoleId = userModelApi.UserRoleId,
                PasswordHash = PasswordHasher.HashPassword(password)
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return Result<UserModelApi>.Success(userModelApi);
        }

        // Создать пользователя при регистрации
        public async Task<Result<UserModelApi>> CreateUserOnRegistrationAsync(UserModelApi? userModelApi, string? password)
        {
            var validation = ValidateUser(userModelApi, password);
            if (!validation.Result.IsSuccess)
                return validation.Result;

            var userRole = await _unitOfWork.UserRoles.GetAll().FirstOrDefaultAsync(r => r.Name == "Заказчик");

            var existingCodes = await _unitOfWork.Users.GetAll()
                .Select(u => u.Code)
                .ToListAsync();

            var newCode = UserCodeGenerator.GenerateUserCode(existingCodes);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Login = userModelApi.Login,
                Name = userModelApi.Name,
                Code = newCode,
                Address = userModelApi.Address,
                Discount = 0,
                UserRoleId = userRole.Id,
                PasswordHash = PasswordHasher.HashPassword(password)
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return Result<UserModelApi>.Success(userModelApi);
        }

        public async Task<Result<UpdateUserModelApi>> UpdateUserAsync(Guid id, UpdateUserModelApi? updateUserModelApi)
        {
            if (updateUserModelApi == null)
                return Result<UpdateUserModelApi>.Fail("Некорректный запрос, данные отсутствуют", 400);

            var user = await _unitOfWork.Users.FindAsync(id);

            if (user == null)
                return Result<UpdateUserModelApi>.Fail("Пользователь не найден", 404);

            if (updateUserModelApi.Discount < 0 || updateUserModelApi.Discount > 100)
                return Result<UpdateUserModelApi>.Fail("Скидка должна быть в диапазоне 0-100", 422);

            if (string.IsNullOrWhiteSpace(updateUserModelApi.Name) || updateUserModelApi.Name.Length > 30)
                return Result<UpdateUserModelApi>.Fail("Некорректное имя", 422);

            if (string.IsNullOrWhiteSpace(updateUserModelApi.Address) || updateUserModelApi.Address.Length > 30)
                return Result<UpdateUserModelApi>.Fail("Некорректный адрес", 422);

            user.Name = updateUserModelApi.Name;
            user.Address = updateUserModelApi.Address;
            user.Discount = updateUserModelApi.Discount;

            await _unitOfWork.SaveChangesAsync();
            return Result<UpdateUserModelApi>.Success(updateUserModelApi);
        }

        public async Task<Result<UpdateUserModelApi>> DeleteUserAsync(Guid id, Guid userId)
        {
            var user = await _unitOfWork.Users.FindAsync(id);

            if (user == null)
                return Result<UpdateUserModelApi>.Fail("Пользователь не найден", 404);

            if (id == userId)
                return Result<UpdateUserModelApi>.Fail("Нельзя удалить самого себя", 400);

            _unitOfWork.Users.Delete(user);
            await _unitOfWork.SaveChangesAsync();

            var deletedUser = new UpdateUserModelApi
            {
                Name = user.Name,
                Address = user.Address,
                Discount = user.Discount,
            };
            return Result<UpdateUserModelApi>.Success(deletedUser);
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <param name="userModelApi"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<Result<UserModelApi>> ValidateUser(UserModelApi? userModelApi, string? password)
        {
            if (userModelApi == null || string.IsNullOrWhiteSpace(password))
                return Result<UserModelApi>.Fail("Некорректный запрос, данные отсутствуют", 400);

            bool loginExists = await _unitOfWork.Users.GetAll().AnyAsync(u => u.Login == userModelApi.Login);

            if (loginExists)
                return Result<UserModelApi>.Fail("Данный логин уже используется", 409);

            if (string.IsNullOrWhiteSpace(userModelApi.Login) || userModelApi.Login.Length > 20)
                return Result<UserModelApi>.Fail("Некорректный логин", 422);

            if (string.IsNullOrWhiteSpace(userModelApi.Name) || userModelApi.Name.Length > 30)
                return Result<UserModelApi>.Fail("Некорректное имя", 422);

            if (string.IsNullOrWhiteSpace(userModelApi.Address) || userModelApi.Address.Length > 30)
                return Result<UserModelApi>.Fail("Некорректный адрес", 422);

            return Result<UserModelApi>.Success(userModelApi);
        }
    }
}
