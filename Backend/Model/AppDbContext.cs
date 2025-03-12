using GoodsAndOrders.Model.Entities;
using Microsoft.EntityFrameworkCore;
using EFCore.NamingConventions;
using Microsoft.Extensions.Configuration;


namespace GoodsAndOrders.Model
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public DbSet<ProductUserOrder> ProductsUserOrders { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserOrder> UserOrders { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseNpgsql(connectionString);
                optionsBuilder.UseSnakeCaseNamingConvention();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderStatus>().ToTable("order_statuses");
            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<ProductCategory>().ToTable("product_categories");
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<UserRole>().ToTable("user_roles");
            modelBuilder.Entity<UserOrder>().ToTable("user_orders");
            modelBuilder.Entity<UserOrder>().Property(o => o.OrderNumber).UseIdentityColumn();
            modelBuilder.Entity<ProductUserOrder>().ToTable("products_user_orders");

            // Настройка удаления "товары" к "заказу"
            modelBuilder.Entity<ProductUserOrder>()
                    .HasOne(puo => puo.UserOrder)
                    .WithMany(o => o.OrderElements)
                    .HasForeignKey(puo => puo.UserOrderId)
                    .OnDelete(DeleteBehavior.Cascade);

            // Настройка удаления "ролей" к "пользователю"
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserRole) 
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.UserRoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
