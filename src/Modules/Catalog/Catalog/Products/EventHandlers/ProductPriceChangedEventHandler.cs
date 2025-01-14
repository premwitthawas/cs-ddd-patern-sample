
using MassTransit;

using Shared.Messaging.Event;

namespace Catalog.Products.EventHandlers;

public class ProductPriceChangedEventHandler(
    IBus bus,
    ILogger<ProductPriceChangedEventHandler> logger
) 
: INotificationHandler<ProductPriceChangedEvent>
{
    public async Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Domain Event Handled : {notification.GetType().Name}");
        // return Task.CompletedTask;
        var intergrateEvente = new ProductPriceChangedIntegrationEvent{
            ProductId = notification.Product.Id,
            Name = notification.Product.Name,
            Category = notification.Product.Category,
            ImageFile = notification.Product.ImageFile,
            Description = notification.Product.Description,
            Price = notification.Product.Price
        };

        await bus.Publish(intergrateEvente, cancellationToken);
    }
}