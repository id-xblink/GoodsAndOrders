using GoodsAndOrders.Model.Entities;
using GoodsAndOrders.Model;
using Microsoft.EntityFrameworkCore;
using GoodsAndOrders.Model.ModelApi;
using GoodsAndOrders.UnitOfWork;
using GoodsAndOrders.Common;

namespace GoodsAndOrders.Services
{
    public class UserRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<UserRoleModelApi>>> GetAllUserRolesAsync()
        {
            var roles = await _unitOfWork.UserRoles.GetAll().ToListAsync();

            if (roles == null || roles.Count == 0)
                return Result<List<UserRoleModelApi>>.Fail("Роли не найдены", 404);

            var list = roles.Select(u => new UserRoleModelApi
            {
                Id = u.Id,
                Name = u.Name,
            }).ToList();

            return Result<List<UserRoleModelApi>>.Success(list);
        }
    }
}