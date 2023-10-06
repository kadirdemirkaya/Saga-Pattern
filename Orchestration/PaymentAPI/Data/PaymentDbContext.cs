using Microsoft.EntityFrameworkCore;
using PaymentAPI.Model;
using SharedLIBRARY.Configurations;

namespace PaymentAPI.Data
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext()
        {
        }
        public PaymentDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetDbSettings().ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
