using Ordering.Domain.Enums;

namespace Ordering.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .IsRequired()
            .HasConversion(orderId => orderId.Value, dbId => OrderId.Of(dbId));

        builder.Property(o => o.CustomerId)
            .IsRequired()
            .HasConversion(customerId => customerId.Value, dbId => CustomerId.Of(dbId));

        builder.ComplexProperty(o => o.OrderName, orderNameBuilder =>
        {
            orderNameBuilder.Property(on => on.Value)
            .HasColumnName(nameof(Order.OrderName))
            .IsRequired()
            .HasMaxLength(100);
        });

        builder.ComplexProperty(o => o.ShippingAddress, addressBuilder =>
        {
            addressBuilder.Property(a => a.FirstName).IsRequired().HasMaxLength(50);
            addressBuilder.Property(a => a.LastName).IsRequired().HasMaxLength(50);
            addressBuilder.Property(a => a.EmailAddress).HasMaxLength(50);
            addressBuilder.Property(a => a.AddressLine).IsRequired().HasMaxLength(180);
            addressBuilder.Property(a => a.State).IsRequired().HasMaxLength(100);
            addressBuilder.Property(a => a.ZipCode).IsRequired().HasMaxLength(5);
            addressBuilder.Property(a => a.Country).IsRequired().HasMaxLength(50);
        });

        builder.ComplexProperty(o => o.BillingAddress, addressBuilder =>
        {
            addressBuilder.Property(a => a.FirstName).IsRequired().HasMaxLength(50);
            addressBuilder.Property(a => a.LastName).IsRequired().HasMaxLength(50);
            addressBuilder.Property(a => a.EmailAddress).HasMaxLength(50);
            addressBuilder.Property(a => a.AddressLine).IsRequired().HasMaxLength(180);
            addressBuilder.Property(a => a.State).IsRequired().HasMaxLength(100);
            addressBuilder.Property(a => a.ZipCode).IsRequired().HasMaxLength(5);
            addressBuilder.Property(a => a.Country).IsRequired().HasMaxLength(50);
        });

        builder.ComplexProperty(o => o.Payment, paymentBuilder =>
        {
            paymentBuilder.Property(p => p.CardName).IsRequired().HasMaxLength(50);
            paymentBuilder.Property(p => p.CardNumber).IsRequired().HasMaxLength(24);
            paymentBuilder.Property(p => p.Expiration).IsRequired().HasMaxLength(10);
            paymentBuilder.Property(p => p.CVV).IsRequired().HasMaxLength(3);
            paymentBuilder.Property(p => p.PaymentMethod).IsRequired();
        });

        builder.Property(o => o.Status)
            .IsRequired()
            .HasDefaultValue(OrderStatus.Draft)
            .HasSentinel(OrderStatus.Draft)
            .HasConversion(
                status => status.ToString(),
                dbStatus => Enum.Parse<OrderStatus>(dbStatus));

        builder.Property(o => o.TotalPrice).HasColumnType("decimal(18,2)");

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .IsRequired();

        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}