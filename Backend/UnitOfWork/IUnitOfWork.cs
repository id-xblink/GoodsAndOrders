using GoodsAndOrders.Abstractions.Repositories;
using GoodsAndOrders.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GoodsAndOrders.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<OrderStatus> OrderStatuses { get; }
        IGenericRepository<Product> Products { get; }
        IGenericRepository<ProductCategory> ProductCategories { get; }
        IGenericRepository<ProductUserOrder> ProductsUserOrders { get; }
        IGenericRepository<User> Users { get; }
        IGenericRepository<UserOrder> UserOrders { get; }
        IGenericRepository<UserRole> UserRoles { get; }
        Task<int> SaveChangesAsync();
    }
}