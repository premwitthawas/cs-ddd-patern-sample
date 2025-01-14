
using Basket.Basket.Features.UpdateItemPriceBasket;

using MassTransit;

using Microsoft.Extensions.Logging;

using Shared.Messaging.Event;

namespace Basket.Basket.EventHandlers;


public class ProductPriceChangedIntegrationEventHandler(
    ISender sender,
    ILogger<ProductPriceChangedIntegrationEventHandler> logger
)
: IConsumer<ProductPriceChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
        logger.LogInformation("Intergration Event Handle : {0}", context.Message.GetType().Name);
        var command = new UpdateItemPriceBasketCommand(context.Message.ProductId, context.Message.Price);
        var result = await sender.Send(command);
        if (!result.IsSuccess)
        {
            logger.LogError("Error Updating price in basket for product id : {0}", context.Message.ProductId);
        }
        logger.LogInformation("Price for product id: {0} updated in basket", context.Message.ProductId);
        await Task.CompletedTask;
    }
}