namespace Basket.Basket.Repositories;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasket(string userName, bool asNotacking = true, CancellationToken cancellationToken = default);
    Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken cancellationToken = default);
    Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default);
}