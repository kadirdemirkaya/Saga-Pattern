using Microsoft.EntityFrameworkCore;
using SharedLIBRARY.Configurations;
using StockAPI.Model;

namespace StockAPI.Data
{
    public class StockDbContext : DbContext
    {
        public StockDbContext()
        {
        }

        public StockDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetDbSettings().ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
