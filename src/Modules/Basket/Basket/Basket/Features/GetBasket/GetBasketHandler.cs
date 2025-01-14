using Basket.Basket.Repositories;

namespace Basket.Basket.Features.GetBasket;

public record GetBasketQueryResult(ShoppingCartDto ShoppingCart);

public record GetBasketQuery(string UserName) : IQuery<GetBasketQueryResult>;
internal class GetBasketHandler(IBasketRepository repository)
: IQueryHandler<GetBasketQuery, GetBasketQueryResult>
{
    public async Task<GetBasketQueryResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basketExists = await repository.GetBasket(request.UserName, true, cancellationToken);
        var basketDto = basketExists.Adapt<ShoppingCartDto>();
        return new GetBasketQueryResult(basketDto);
    }
};