using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrderAPI.Models;
using SharedLIBRARY.Configurations;

namespace OrderAPI.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext()
        {
           
        }

        public OrderDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetDbSettings().ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
