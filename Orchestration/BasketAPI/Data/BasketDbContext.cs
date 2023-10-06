using BasketAPI.Models;
using Microsoft.EntityFrameworkCore;
using SharedLIBRARY.Configurations;

namespace BasketAPI.Data
{
    public class BasketDbContext : DbContext
    {
        public BasketDbContext()
        {
        }

        public BasketDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetDbSettings().ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
