namespace Ordering.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).HasConversion(
            customerId => customerId.Value,
            dbId => ProductId.Of(dbId));

        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

        builder.Property(c => c.Price).IsRequired()
            .HasColumnType("decimal(18,2)");
    }
}