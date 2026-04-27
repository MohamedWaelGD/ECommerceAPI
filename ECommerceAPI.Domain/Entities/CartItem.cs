namespace ECommerceAPI.Domain.Entities;

public class CartItem
{
    private CartItem() { }

    private CartItem(Guid id, Guid cartId, Product product, int quantity)
    {
        Id = id;
        CartId = cartId;
        ProductId = product.Id;
        Product = product;
        Quantity = quantity;
        UnitPriceSnapshot = product.Price;
    }

    public Guid Id { get; private set; }
    public Guid CartId { get; private set; }
    public Cart Cart { get; private set; } = default!;
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = default!;
    public int Quantity { get; private set; }
    public decimal UnitPriceSnapshot { get; private set; }

    public static CartItem Create(Guid cartId, Product product, int quantity) => new(Guid.NewGuid(), cartId, product, quantity);

    public void UpdateQuantity(int quantity, int stockQuantity)
    {
        if (quantity <= 0) throw new InvalidOperationException("Quantity must be greater than zero.");
        if (quantity > stockQuantity) throw new InvalidOperationException("Quantity cannot exceed available stock.");

        Quantity = quantity;
    }

    public void UpdatePriceSnapshot(decimal unitPrice) => UnitPriceSnapshot = unitPrice;
}
