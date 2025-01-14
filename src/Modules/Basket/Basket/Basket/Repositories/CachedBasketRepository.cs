using System.Text.Json;
using System.Text.Json.Serialization;

using Basket.Basket.JsonConvertors;

using Microsoft.Extensions.Caching.Distributed;
namespace Basket.Basket.Repositories;


public class CachedBasketRepository(
    IBasketRepository repository,
    IDistributedCache cache
    ) : IBasketRepository
{
    private readonly JsonSerializerOptions _option = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new ShoppingCartConverter(), new ShoppingCartItemConverter() }
    };
    public async Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await repository.CreateBasket(basket, cancellationToken);
        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket, _option), cancellationToken);
        return basket;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        await repository.DeleteBasket(userName, cancellationToken);
        await cache.RemoveAsync(userName, cancellationToken);
        return true;
    }

    public async Task<ShoppingCart> GetBasket(string userName, bool asNotacking = true, CancellationToken cancellationToken = default)
    {
        if (!asNotacking)
        {
            return await repository.GetBasket(userName, false, cancellationToken);
        }
        var cacheBesket = await cache.GetStringAsync(userName, cancellationToken);
        if (!string.IsNullOrEmpty(cacheBesket))
        {
            return JsonSerializer.Deserialize<ShoppingCart>(cacheBesket, _option)!;
        }
        var basket = await repository.GetBasket(userName, asNotacking, cancellationToken);
        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket, _option), cancellationToken);
        return basket;
    }

    public async Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default)
    {
        // TODO: clear cache
        var result = await repository.SaveChangesAsync(userName, cancellationToken);
        if (userName is not null)
        {
            await cache.RemoveAsync(userName, cancellationToken);
        }
        return result;
    }
}