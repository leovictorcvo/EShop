namespace Ordering.Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.Id)
            .IsRequired()
            .HasConversion(orderItemId => orderItemId.Value, dbId => OrderItemId.Of(dbId));

        builder.Property(oi => oi.OrderId)
            .IsRequired()
            .HasConversion(orderId => orderId.Value, dbId => OrderId.Of(dbId));

        builder.Property(oi => oi.ProductId)
            .IsRequired()
            .HasConversion(productId => productId.Value, dbId => ProductId.Of(dbId));

        builder.Property(oi => oi.Price).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(oi => oi.Quantity).IsRequired();

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}