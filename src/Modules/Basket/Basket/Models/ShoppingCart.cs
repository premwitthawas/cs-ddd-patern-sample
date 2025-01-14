namespace Basket.Models;

public class ShoppingCart : Aggregate<Guid>
{
    public string UserName { get; private set; } = default!;
    private readonly List<ShoppingCartItem> _items = new();
    public IReadOnlyList<ShoppingCartItem> Items => _items.AsReadOnly();
    public decimal TotalPrice => _items.Sum(x => x.Price * x.Quantity);

    public static ShoppingCart Create(Guid id, string userName)
    {
        ArgumentException.ThrowIfNullOrEmpty(userName);

        var shoppingCart = new ShoppingCart
        {
            Id = id,
            UserName = userName
        };
        return shoppingCart;
    }

    public void AddItem(Guid productId, int quantity,
     string color, decimal price, string productName)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var exsitingItem = Items.FirstOrDefault(x => x.ProductId == productId);
        if (exsitingItem != null)
        {
            exsitingItem.Quantity += quantity;
        }
        else
        {
            var newItem = new ShoppingCartItem(
                Id,
                productId,
                productName,
                price,
                color,
                quantity
            );
            _items.Add(newItem);
        }
    }

    public void RemoveItem(Guid productId)
    {
        var existingItem = Items.FirstOrDefault(x => x.ProductId == productId);
        if (existingItem != null)
        {
            _items.Remove(existingItem);
        }
    }
}