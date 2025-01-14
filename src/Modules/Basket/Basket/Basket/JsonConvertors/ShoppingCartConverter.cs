using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Basket.Basket.JsonConvertors;

public class ShoppingCartConverter : JsonConverter<ShoppingCart>
{
    public override ShoppingCart? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonDocument = JsonDocument.ParseValue(ref reader);
        var root = jsonDocument.RootElement;

        var id = root.GetProperty("id").GetGuid();
        var userName = root.GetProperty("userName").GetString();
        var itemsElement = root.GetProperty("items");

        var shoppingCart = ShoppingCart.Create(id, userName!);

        var items = itemsElement.Deserialize<List<ShoppingCartItem>>(options);
        if (items != null)
        {
            var itemField = typeof(ShoppingCart).GetField("_items", BindingFlags.NonPublic);
            itemField?.SetValue(shoppingCart, items);
        }
        return shoppingCart;
    }

    public override void Write(Utf8JsonWriter writer, ShoppingCart value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("id", value.Id);
        writer.WriteString("userName", value.UserName);
        writer.WritePropertyName("items");
        JsonSerializer.Serialize(writer, value.Items, options);
        writer.WriteEndObject();
    }
}