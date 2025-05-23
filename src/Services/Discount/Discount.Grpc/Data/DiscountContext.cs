using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountContext : DbContext
{
    public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
    {
    }

    public DbSet<Coupon> Coupons => Set<Coupon>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>().HasKey(c => c.Id);
        modelBuilder.Entity<Coupon>().Property(c => c.ProductName).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<Coupon>().Property(c => c.Description).IsRequired().HasMaxLength(200);
        modelBuilder.Entity<Coupon>().Property(c => c.Amount).IsRequired();
        modelBuilder.Entity<Coupon>().ToTable("Coupons");

        modelBuilder.Entity<Coupon>().HasData(
            new Coupon
            {
                Id = 1,
                ProductName = "IPhone",
                Description = "IPhone Discount",
                Amount = 150
            },
            new Coupon
            {
                Id = 2,
                ProductName = "Samsung S25",
                Description = "Samsung Discount",
                Amount = 100
            });
    }
}