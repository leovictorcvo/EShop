namespace Ordering.Domain.Models;

public class Product : Entity<ProductId>
{
    public string Name { get; init; } = default!;

    public decimal Price { get; init; } = default!;

    public static Product Create(ProductId id, string name, decimal price)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        return new Product
        {
            Id = id,
            Name = name,
            Price = price
        };
    }
}