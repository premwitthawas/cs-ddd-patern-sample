namespace Basket.Basket.Features.CreateBasket;

public record CreateBasketCommand(ShoppingCartDto ShoppingCart) : ICommand<CreateBasketResult>;

public record CreateBasketResult(Guid Id);

public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
{
    public CreateBasketCommandValidator()
    {
       RuleFor(x => x.ShoppingCart.UserName).NotNull().WithMessage("UserName is required");
    }
}

internal class CreateBasketHandler(IBasketRepository repository)
: ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    public async Task<CreateBasketResult> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = CreateNewShopingCart(command.ShoppingCart);
        await repository.CreateBasket(shoppingCart, cancellationToken);
        return new CreateBasketResult(shoppingCart.Id);
    }

    private ShoppingCart CreateNewShopingCart(ShoppingCartDto shoppingCart)
    {
        var newBasket = ShoppingCart.Create(Guid.NewGuid(), shoppingCart.UserName);
        return newBasket;
    }
}