namespace Ordering.Application.Extensions;

internal static class OrderExtensions
{
    internal static IEnumerable<OrderDto> ProjectToOrderDto(this IEnumerable<Order> orders)
    {
        foreach (var order in orders)
        {
            var shippingAddress = new AddressDto(
                order.ShippingAddress.FirstName,
                order.ShippingAddress.LastName,
                order.ShippingAddress.EmailAddress,
                order.ShippingAddress.AddressLine,
                order.ShippingAddress.Country,
                order.ShippingAddress.State,
                order.ShippingAddress.ZipCode);

            var billingAddress = new AddressDto(
                order.BillingAddress.FirstName,
                order.BillingAddress.LastName,
                order.BillingAddress.EmailAddress,
                order.BillingAddress.AddressLine,
                order.BillingAddress.Country,
                order.BillingAddress.State,
                order.BillingAddress.ZipCode);

            var paymentDto = new PaymentDto(
                order.Payment.CardName!,
                order.Payment.CardNumber,
                order.Payment.Expiration,
                order.Payment.CVV,
                order.Payment.PaymentMethod);

            yield return new OrderDto(
                order.Id.Value,
                order.CustomerId.Value,
                order.OrderName.Value,
                shippingAddress,
                billingAddress,
                paymentDto,
                order.Status,
                [.. order.OrderItems.Select(oi => new OrderItemDto(oi.OrderId.Value, oi.ProductId.Value, oi.Quantity, oi.Price))]
            );
        }
    }
}