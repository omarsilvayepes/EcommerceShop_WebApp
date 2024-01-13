using CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CouponAPI.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

        public DbSet<Coupon> Coupons { get; set; }

        // set some data(seed)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData(
                new Coupon { CuponId = 1, CuponCode="10OFF",DiscountAmount=10,MinAmount=20},
                new Coupon { CuponId = 2, CuponCode = "20OFF", DiscountAmount = 20, MinAmount = 40 }
                );
        }
    }
}
