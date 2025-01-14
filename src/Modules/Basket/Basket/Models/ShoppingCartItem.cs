using System.Text.Json.Serialization;

namespace Basket.Models;

public class ShoppingCartItem : Entity<Guid>
{
    public Guid ShoppingCartId { get; private set; } = default!;
    public Guid ProductId { get; private set; } = default!;
    public int Quantity { get; internal set; } = default!;
    public string Color { get; private set; } = default!;
    public decimal Price { get; private set; } = default!;
    public string ProductName { get; private set; } = default!;

    internal ShoppingCartItem(Guid shoppingCartId, Guid productId, string productName, decimal price, string color, int quantity)
    {
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
        ProductName = productName;
        Price = price;
        Color = color;
        Quantity = quantity;
    }

    [JsonConstructor]
    public ShoppingCartItem(Guid id, Guid shoppingCartId, Guid productId, string productName, decimal price, string color, int quantity)
    {
        Id = id;
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
        ProductName = productName;
        Price = price;
        Color = color;
        Quantity = quantity;
    }

    public void UpdatePrice(decimal newPrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(newPrice);
        Price = newPrice;
    }
}