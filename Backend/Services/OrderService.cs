using GoodsAndOrders.Model;
using GoodsAndOrders.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata.Ecma335;
using GoodsAndOrders.Model.ModelApi;
using GoodsAndOrders.UnitOfWork;
using GoodsAndOrders.Common;

namespace GoodsAndOrders.Services
{
    public class OrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public OrderService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<Result<object>> GetAllOrdersAsync(Guid? customerId, int page, int pageSize, Guid? orderStatusId)
        {
            if (page < 1 || pageSize < 1)
                return Result<object>.Fail("Некорректные параметры пагинации", 400);

            var query = _unitOfWork.UserOrders.GetAll()
                .Include(o => o.OrderStatus)
                .AsQueryable();

            if (customerId.HasValue)
            {
                query = query.Where(o => o.CustomerId == customerId.Value);
            }

            if (orderStatusId.HasValue)
            {
                query = query.Where(o => o.StatusId == orderStatusId.Value);
            }
            int totalItems = await query.CountAsync();

            var orders = await query
                .OrderBy(o => o.OrderNumber)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new OrderResponseModelApi
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    OrderNumber = o.OrderNumber,
                    ShipmentDate = o.ShipmentDate,
                    Status = o.OrderStatus.Name
                })
                .ToListAsync();

            var data = new
            {
                orders,
                totalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                totalItems
            };

            return Result<object>.Success(data);
        }

        public async Task<Result<OrderModelApi>> GetOrderByIdAsync(Guid id)
        {
            var order = await _unitOfWork.UserOrders.GetAll()
                .Include(o => o.OrderElements)  // Загружаем товары в заказе
                .ThenInclude(puo => puo.Product) // Загружаем сами товары
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return Result<OrderModelApi>.Fail("Заказ не найден", 404);

            var data = new OrderModelApi
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                ShipmentDate = order.ShipmentDate,
                OrderNumber = order.OrderNumber,
                CustomerId = order.CustomerId,
                StatusId = order.StatusId,
                Items = order.OrderElements.Select(puo => new OrderProductItem
                {
                    ProductId = puo.ProductId,
                    ProductName = puo.Product.Name,
                    Price = puo.ProductPrice,
                    Quantity = puo.ProductCount
                }).ToList()
            };

            return Result<OrderModelApi>.Success(data);
        }

        public async Task<Result<OrderResponseModelApi>> CreateOrderAsync(CreateOrderModelApi? createOrderModelApi)
        {
            var newStatusName = _configuration["DefaultStatuses:New"];

            if (createOrderModelApi == null || createOrderModelApi.Items.Count == 0)
                return Result<OrderResponseModelApi>.Fail("Некорректный запрос, данные отсутствуют", 400);

            var orderStatus = await _unitOfWork.OrderStatuses.GetAll()
                .FirstOrDefaultAsync(s => s.Name == newStatusName);

            // Загружаем заказчика с его скидкой
            var customer = await _unitOfWork.Users.GetAll()
                .Where(u => u.Id == createOrderModelApi.CustomerId)
                .Select(u => new { u.Id, u.Discount })
                .FirstOrDefaultAsync();

            var order = new UserOrder
            {
                Id = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                OrderNumber = await _unitOfWork.UserOrders.GetAll().CountAsync() + 1,
                CustomerId = createOrderModelApi.CustomerId,
                StatusId = orderStatus.Id,
                OrderStatus = orderStatus
            };

            await _unitOfWork.UserOrders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            var orderProducts = createOrderModelApi.Items.Select(item =>
            {
                var productPrice = _unitOfWork.Products.GetAll()
                                        .Where(p => p.Id == item.ProductId)
                                        .Select(p => p.Price)
                                        .FirstOrDefault();

                return new ProductUserOrder
                {
                    Id = Guid.NewGuid(),
                    UserOrderId = order.Id,
                    ProductId = item.ProductId,
                    ProductCount = item.Quantity,
                    ProductPrice = CalculateDiscountedPrice(productPrice, customer.Discount),
                };
            }).ToList();

            await _unitOfWork.ProductsUserOrders.AddRangeAsync(orderProducts);
            await _unitOfWork.SaveChangesAsync();

            var orderResponseModelApi = new OrderResponseModelApi()
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                OrderNumber = order.OrderNumber,
                Status = order.OrderStatus.Name,
            };

            return Result<OrderResponseModelApi>.Success(orderResponseModelApi);
        }

        private decimal CalculateDiscountedPrice(decimal originalPrice, decimal discount)
        {
            return originalPrice * (1 - (discount / 100m));
        }

        public async Task<Result<UpdateOrderModelApi>> UpdateOrderStatusAsync(UpdateOrderModelApi? updateOrderModelApi)
        {
            if (updateOrderModelApi == null)
                return Result<UpdateOrderModelApi>.Fail("Некорректный запрос, данные отсутствуют", 400);

            var order = await _unitOfWork.UserOrders.GetAll().Include(o => o.OrderStatus).FirstOrDefaultAsync(o => o.Id == updateOrderModelApi.Id);

            if (order == null)
                return Result<UpdateOrderModelApi>.Fail("Заказ не найден", 404);

            var statuses = _unitOfWork.OrderStatuses.GetAll();

            // Когда подготавливаем заказ
            if (order.OrderStatus.Name == "Новый")
            {
                if (!updateOrderModelApi.ShipmentDate.HasValue || updateOrderModelApi.ShipmentDate.Value.Date < DateTime.UtcNow.Date)
                    return Result<UpdateOrderModelApi>.Fail("Дата отгрузки должна быть сегодня или позже", 422);

                var orderStatus = statuses.FirstOrDefault(s => s.Name == "Выполняется");
                order.StatusId = orderStatus.Id;
                order.ShipmentDate = updateOrderModelApi.ShipmentDate.Value;
            }
            // Когда закрываем заказ
            if (order.OrderStatus.Name == "Выполняется")
            {
                if (!order.ShipmentDate.HasValue || order.ShipmentDate.Value.Date > DateTime.UtcNow.Date)
                    return Result<UpdateOrderModelApi>.Fail("Нельзя закрыть заказ до даты отгрузки", 422);

                var orderStatus = statuses.FirstOrDefault(s => s.Name == "Выполнен");
                order.StatusId = orderStatus.Id;
            }
            if (order.OrderStatus.Name == "Выполнен")
            {
                return Result<UpdateOrderModelApi>.Fail("Некорректный запрос, нельзя изменить выполненый заказ", 400);
            }
            await _unitOfWork.SaveChangesAsync();

            return Result<UpdateOrderModelApi>.Success(updateOrderModelApi);
        }


        public async Task<Result<UpdateOrderModelApi>> DeleteOrderAsync(Guid id)
        {
            var order = await _unitOfWork.UserOrders.GetAll().Include(o => o.OrderStatus).FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return Result<UpdateOrderModelApi>.Fail("Заказ не найден", 404);

            if (order.OrderStatus.Name != "Новый")
                return Result<UpdateOrderModelApi>.Fail("Заказ нельзя удалить", 400);

            _unitOfWork.UserOrders.Delete(order);
            await _unitOfWork.SaveChangesAsync();

            var deletedOrder = new UpdateOrderModelApi
            {
                Id = order.Id,
                ShipmentDate = null,
            };

            return Result<UpdateOrderModelApi>.Success(deletedOrder);
        }
    }
}
