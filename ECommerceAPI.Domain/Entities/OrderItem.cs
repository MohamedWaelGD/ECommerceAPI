namespace ECommerceAPI.Domain.Entities;

public class OrderItem
{
    private OrderItem() { }

    private OrderItem(Guid id, Guid productId, string productNameSnapshot, decimal unitPrice, int quantity)
    {
        Id = id;
        ProductId = productId;
        ProductNameSnapshot = productNameSnapshot;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = default!;
    public Guid ProductId { get; private set; }
    public Product? Product { get; private set; }
    public string ProductNameSnapshot { get; private set; } = string.Empty;
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal TotalPrice => UnitPrice * Quantity;

    public static OrderItem Create(CartItem cartItem) =>
        new(Guid.NewGuid(), cartItem.ProductId, cartItem.Product.Name, cartItem.UnitPriceSnapshot, cartItem.Quantity);
}
