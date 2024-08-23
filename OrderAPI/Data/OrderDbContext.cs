using System.Collections.Generic;
using System.Reflection.Emit;

using Microsoft.EntityFrameworkCore;

using OrderAPI.Models;

namespace OrderAPI.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>().HasKey(o => o.OrderId);
            modelBuilder.Entity<OrderItem>().HasKey(oi => oi.ItemId);
            modelBuilder.Entity<Order>().HasMany(o => o.Items).WithOne().HasForeignKey(i => i.OrderId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
