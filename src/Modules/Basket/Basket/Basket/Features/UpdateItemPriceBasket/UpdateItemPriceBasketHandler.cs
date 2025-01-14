
namespace Basket.Basket.Features.UpdateItemPriceBasket;

public record UpdateItemPriceBasketCommand(Guid ProductId, decimal Price) : ICommand<UpdateItemPriceBasketResult>;
public record UpdateItemPriceBasketResult(bool IsSuccess);

public class UpdateItemPriceBasketCommandValidator : AbstractValidator<UpdateItemPriceBasketCommand>
{
    public UpdateItemPriceBasketCommandValidator()
    {
        RuleFor(r => r.ProductId).NotEmpty().WithMessage("ProductId is Required");
        RuleFor(r => r.Price).GreaterThan(0).WithMessage("Price is Greaterthan 0");
    }
}

internal class UpdateItemPriceBasketHandler(BasketDbContext dbContext)
: ICommandHandler<UpdateItemPriceBasketCommand, UpdateItemPriceBasketResult>
{
    public async Task<UpdateItemPriceBasketResult> Handle(UpdateItemPriceBasketCommand command, CancellationToken cancellationToken)
    {
        var itemsToUpdate = await dbContext.ShoppingCartItems
        .Where(x => x.ProductId == command.ProductId)
        .ToListAsync(cancellationToken);

        if (!itemsToUpdate.Any())
        {
            return new UpdateItemPriceBasketResult(false);
        }

        foreach (var item in itemsToUpdate)
        {
            item.UpdatePrice(command.Price);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateItemPriceBasketResult(true);

    }
};