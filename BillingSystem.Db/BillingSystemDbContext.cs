using BillingSystem.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BillingSystem.Db
{
    public class BillingSystemDbContext : DbContext
    {
        public BillingSystemDbContext(DbContextOptions<BillingSystemDbContext> options) : base(options)
        { }

        public DbSet<GameItem> GameItems  { get; set; }
        public DbSet<Coupon> Coupon { get; set; }

    }
}
