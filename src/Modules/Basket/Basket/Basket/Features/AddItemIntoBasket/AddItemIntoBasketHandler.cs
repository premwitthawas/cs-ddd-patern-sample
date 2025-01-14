namespace Basket.Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketResult(Guid Id);

public record AddItemIntoBasketCommand(string UserName, ShoppingCartItemDto ShoppingCartItem) : ICommand<AddItemIntoBasketResult>;

public class AddItemIntoBasketCommandValidator : AbstractValidator<AddItemIntoBasketCommand>
{
    public AddItemIntoBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is Required");
        RuleFor(x => x.ShoppingCartItem.ProductId).NotEmpty().WithMessage("ProductId is Required");
        // RuleFor(x => x.ShoppingCartItem.ProductId).NotEmpty().WithMessage("ProductId is Required");
        RuleFor(x => x.ShoppingCartItem.Quantity).GreaterThan(0).WithMessage("Quantity should be greater than 0");
        // RuleFor(x => x.ShoppingCartItem.Price).GreaterThan(0).WithMessage("Price should be greater than 0");
    }
}

internal class AddItemIntoBasketHandler(IBasketRepository repository, ISender sender)
: ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = await repository.GetBasket(command.UserName, false, cancellationToken);
        var result = await sender.Send(new GetProductByIdQuery(command.ShoppingCartItem.ProductId), cancellationToken);
        shoppingCart.AddItem(
            command.ShoppingCartItem.ProductId,
            command.ShoppingCartItem.Quantity,
            command.ShoppingCartItem.Color,
            result.Product.Price,
            result.Product.Name
        );
        await repository.SaveChangesAsync(command.UserName, cancellationToken);
        return new AddItemIntoBasketResult(shoppingCart.Id);
    }
}