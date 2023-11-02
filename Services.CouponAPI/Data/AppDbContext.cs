using BackEnd.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.CouponAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Coupon> coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            List<Coupon> coupons = new List<Coupon> {
                new Coupon()
                {
                    Id = "7b773500-f4a2-4737-b14c-d909a4c71b10",
                    Code = "10OFF",
                    Discount = 10_000,
                    MinAmount = 20_000,
                },
                new Coupon()
                {
                    Id = "cbd2971c-ce9a-47b4-a3f0-e0dd319885ba",
                    Code = "20OFF",
                    Discount = 20_000,
                    MinAmount = 40_000,
                }
            };
            modelBuilder.Entity<Coupon>().HasData(coupons);
        }
    }
}
