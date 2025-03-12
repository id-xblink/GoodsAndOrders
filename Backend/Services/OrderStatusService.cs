using GoodsAndOrders.Common;
using GoodsAndOrders.Model.Entities;
using GoodsAndOrders.UnitOfWork;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;

namespace GoodsAndOrders.Services
{
    public class OrderStatusService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<OrderStatus>>> GetAllStatusesAsync()
        {
            var statuses = await _unitOfWork.OrderStatuses.GetAllAsync();

            if (statuses == null || !statuses.Any())
                return Result<IEnumerable<OrderStatus>>.Fail("Статусы не найдены", 404);

            return Result<IEnumerable<OrderStatus>>.Success(statuses);
        }
    }
}
