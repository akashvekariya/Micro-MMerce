namespace Basket.API.Models;

public class ShoppingCartItem
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Color { get; set; }

    public ShoppingCartItem() { }

    public ShoppingCartItem(
        string productId, string productName, decimal price, int quantity, string color
    )
    {
        ProductId = productId;
        ProductName = productName;
        Price = price;
        Quantity = quantity;
        Color = color;
    }
}
