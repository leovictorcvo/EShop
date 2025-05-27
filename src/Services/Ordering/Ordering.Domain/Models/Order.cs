namespace Ordering.Domain.Models;

public class Order : Aggregate<OrderId>
{
    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public CustomerId CustomerId { get; private set; } = default!;
    public OrderName OrderName { get; private set; } = default!;
    public Address ShippingAddress { get; private set; } = default!;
    public Address BillingAddress { get; private set; } = default!;
    public Payment Payment { get; private set; } = default!;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;

    public decimal TotalPrice
    {
        get => _orderItems.Sum(item => item.Price * item.Quantity);
#pragma warning disable S1144 // Unused private types or members should be removed
#pragma warning disable S3237 // "value" contextual keyword should be used
        private set { /* Used by EF */ }
#pragma warning restore S3237 // "value" contextual keyword should be used
#pragma warning restore S1144 // Unused private types or members should be removed
    }

    public static Order Create(
        OrderId id,
        CustomerId customerId,
        OrderName orderName,
        Address shippingAddress,
        Address billingAddress,
        Payment payment)
    {
        ArgumentNullException.ThrowIfNull(customerId);
        ArgumentNullException.ThrowIfNull(orderName);
        ArgumentNullException.ThrowIfNull(shippingAddress);
        ArgumentNullException.ThrowIfNull(billingAddress);
        ArgumentNullException.ThrowIfNull(payment);
        var order = new Order
        {
            Id = id,
            CustomerId = customerId,
            OrderName = orderName,
            ShippingAddress = shippingAddress,
            BillingAddress = billingAddress,
            Payment = payment,
            Status = OrderStatus.Pending
        };
        order.AddDomainEvent(new OrderCreatedEvent(order));
        return order;
    }

    public void Update(
        OrderName orderName,
        Address shippingAddress,
        Address billingAddress,
        Payment payment,
        OrderStatus status)
    {
        ArgumentNullException.ThrowIfNull(orderName);
        ArgumentNullException.ThrowIfNull(shippingAddress);
        ArgumentNullException.ThrowIfNull(billingAddress);
        ArgumentNullException.ThrowIfNull(payment);

        OrderName = orderName;
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
        Payment = payment;
        Status = status;

        AddDomainEvent(new OrderUpdatedEvent(this));
    }

    public void AddOrderItem(ProductId productId, int quantity, decimal price)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var orderItem = new OrderItem(Id, productId, price, quantity);
        _orderItems.Add(orderItem);
    }

    public void RemoveOrderItem(ProductId productId)
    {
        ArgumentNullException.ThrowIfNull(productId);
        var orderItem = _orderItems.FirstOrDefault(item => item.ProductId == productId) ??
            throw new DomainException($"Order item with ProductId {productId} not found.");

        _orderItems.Remove(orderItem);
    }
}