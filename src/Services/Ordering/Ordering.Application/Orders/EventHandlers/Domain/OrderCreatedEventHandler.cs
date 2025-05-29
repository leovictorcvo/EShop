using Microsoft.FeatureManagement;

namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderCreatedEventHandler(
    ILogger<OrderCreatedEventHandler> logger,
    IFeatureManager featureManager) : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Order Created Event Handled: {OrderId}", notification.Order.Id);

        if (await featureManager.IsEnabledAsync("OrderFullfilment"))
        {
            logger.LogInformation("OrderFullfilment feature is enabled. Should dispatch OrderCreatedIntegrationEvent");
        }
    }
}