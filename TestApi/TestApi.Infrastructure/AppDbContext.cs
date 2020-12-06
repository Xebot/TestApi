using Microsoft.EntityFrameworkCore;
using TestApi.Domain.Entities;

namespace TestApi.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
