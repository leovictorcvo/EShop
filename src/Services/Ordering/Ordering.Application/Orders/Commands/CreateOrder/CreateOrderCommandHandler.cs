namespace Ordering.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = CreateOrder(command.Order);
        await context.Orders.AddAsync(order, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return new CreateOrderResult(order.Id.Value);
    }

    private static Order CreateOrder(OrderDto orderDto)
    {
        var shippingAddressDto = orderDto.ShippingAddress;
        var billingAddressDto = orderDto.BillingAddress;
        var paymentDto = orderDto.Payment;
        var shippingAddress = Address.Of(
            shippingAddressDto.FirstName,
            shippingAddressDto.LastName,
            shippingAddressDto.EmailAddresss,
            shippingAddressDto.AddressLine,
            shippingAddressDto.Country,
            shippingAddressDto.State,
            shippingAddressDto.ZipCode);
        var billingAddress = Address.Of(
            billingAddressDto.FirstName,
            billingAddressDto.LastName,
            billingAddressDto.EmailAddresss,
            billingAddressDto.AddressLine,
            billingAddressDto.Country,
            billingAddressDto.State,
            billingAddressDto.ZipCode);
        var payment = Payment.Of(
            paymentDto.CardName,
            paymentDto.CardNumber,
            paymentDto.Expiration,
            paymentDto.Cvv,
            paymentDto.PaymentMethod);
        var order = Order.Create(
            OrderId.Of(orderDto.Id),
            CustomerId.Of(orderDto.CustomerId),
            OrderName.Of(orderDto.OrderName),
            shippingAddress,
            billingAddress,
            payment);
        foreach (var item in orderDto.OrderItems)
        {
            order.AddOrderItem(ProductId.Of(item.ProductId), item.Quantity, item.Price);
        }
        return order;
    }
}