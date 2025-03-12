using GoodsAndOrders.Abstractions.Repositories;
using GoodsAndOrders.Model;
using GoodsAndOrders.Model.Entities;
using GoodsAndOrders.Repositories;
using System;

namespace GoodsAndOrders.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IGenericRepository<OrderStatus>? _orderStatuses;
        private IGenericRepository<Product>? _products;
        private IGenericRepository<ProductCategory>? _productCategories;
        private IGenericRepository<ProductUserOrder>? _productUserOrders;
        private IGenericRepository<User>? _users;
        private IGenericRepository<UserOrder>? _userOrders;
        private IGenericRepository<UserRole>? _userRoles;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<OrderStatus> OrderStatuses => _orderStatuses ??= new GenericRepository<OrderStatus>(_context);
        public IGenericRepository<Product> Products => _products ??= new GenericRepository<Product>(_context);
        public IGenericRepository<ProductCategory> ProductCategories => _productCategories ??= new GenericRepository<ProductCategory>(_context);
        public IGenericRepository<ProductUserOrder> ProductsUserOrders => _productUserOrders ??= new GenericRepository<ProductUserOrder>(_context);
        public IGenericRepository<User> Users => _users ??= new GenericRepository<User>(_context);
        public IGenericRepository<UserOrder> UserOrders => _userOrders ??= new GenericRepository<UserOrder>(_context);
        public IGenericRepository<UserRole> UserRoles => _userRoles ??= new GenericRepository<UserRole>(_context);


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}