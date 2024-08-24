using Microsoft.EntityFrameworkCore;

using OrderAPI.Models;
using OrderAPI.DTO;

namespace OrderAPI.Services
{
    public class OrderServiceDb : DbContext, IOrderService
    {
        private DbSet<Order> orders { get; set; }

        private DbSet<OrderItem> orderItems { get; set; }

        private DbSet<Product> products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=orders.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(entity => entity.OrderId);
                entity.Property(entity => entity.CustomerName).IsRequired();
                entity.HasMany(entity => entity.Items)
                      .WithOne(entity => entity.Order)
                      .HasForeignKey(entity => entity.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(entity => entity.OrderItemId);
                entity.HasOne(entity => entity.Product)
                      .WithMany()
                      .HasForeignKey(entity => entity.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);


                entity.HasOne(entity => entity.Order)
                      .WithMany(entity => entity.Items)
                      .HasForeignKey(entity => entity.OrderId);
            });
            base.OnModelCreating(modelBuilder);
        }

        public async Task CreateProductAsync(Product product)
        {
            await products.AddAsync(product);
            await SaveChangesAsync();
        }
        public async Task DestroyProductAsync(int productId)
        {
            var product = await products.FindAsync(productId);
            if (product != null)
            {
                products.Remove(product);
                await SaveChangesAsync();
            }
        }
        public async Task CreateOrderAsync(Order order)
        {
            await orders.AddAsync(order);
            await SaveChangesAsync();
        }
        public async Task UpdateOrderAsync(int orderId, Action<Order> updateFunc)
        {
            var order = await orders.FindAsync(orderId);
            if (order != null)
            {
                updateFunc(order);
                await SaveChangesAsync();
            }   
        }
        public async Task<bool> OrderExists(int orderId)
        {
            return await orders.FindAsync(orderId) != null;
        }
        public async Task<ICollection<OrderDto>> GetAllOrdersAsync()
        {
            return await orders.Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                CustomerName = o.CustomerName,
                CreatedDateTime = o.CreatedDate,
                Status = o.Status,
                Items = o.Items.Select(i => new OrderItemDto
                {
                    ProductName = i.Product.ProductName, // Assuming ProductName is included
                    Quantity = i.Quantity,
                    PricePerUnit = i.Product.PricePerUnit
                }).ToList()
            }).ToListAsync();
        }
        public async Task<OrderDto?> GetOrderAsync(int orderId)
        {
            return await orders
                .Where(o => o.OrderId == orderId)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    CustomerName = o.CustomerName,
                    CreatedDateTime = o.CreatedDate,
                    Status = o.Status,
                    Items = o.Items.Select(i => new OrderItemDto
                    {
                        ProductName = i.Product.ProductName, // Assuming ProductName is included
                        Quantity = i.Quantity,
                        PricePerUnit = i.Product.PricePerUnit
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }
        public async Task<Product?> GetProductAsync(int productId)
        {
            return await products.FindAsync(productId);
        }

    }
}
