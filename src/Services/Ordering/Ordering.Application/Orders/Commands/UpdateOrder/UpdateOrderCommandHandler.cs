namespace Ordering.Application.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler(IApplicationDbContext context) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(command.Order.Id);
        var order = await context.Orders.FindAsync([orderId], cancellationToken) ??
            throw new OrderNotFoundException(command.Order.Id);
        UpdateOrder(order, command.Order);
        context.Orders.Update(order);
        await context.SaveChangesAsync(cancellationToken);
        return new UpdateOrderResult(true);
    }

    private static void UpdateOrder(Order order, OrderDto orderDto)
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
        order.Update(
            OrderName.Of(orderDto.OrderName),
            shippingAddress,
            billingAddress,
            payment,
            orderDto.Status);
    }
}