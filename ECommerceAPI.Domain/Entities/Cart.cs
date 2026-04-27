namespace ECommerceAPI.Domain.Entities;

public class Cart
{
    private readonly List<CartItem> _items = [];

    private Cart() { }

    private Cart(Guid id, Guid userId)
    {
        Id = id;
        UserId = userId;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; } = default!;
    public ICollection<CartItem> Items => _items;

    public static Cart Create(Guid userId) => new(Guid.NewGuid(), userId);

    public void AddItem(Product product, int quantity)
    {
        if (!product.IsActive) throw new InvalidOperationException("Cannot add inactive product.");
        if (quantity <= 0) throw new InvalidOperationException("Quantity must be greater than zero.");

        var existing = _items.FirstOrDefault(x => x.ProductId == product.Id);
        var newQuantity = quantity + (existing?.Quantity ?? 0);
        if (!product.HasStock(newQuantity)) throw new InvalidOperationException("Quantity cannot exceed available stock.");

        if (existing is null)
        {
            _items.Add(CartItem.Create(Id, product, quantity));
            return;
        }

        existing.UpdateQuantity(newQuantity, product.StockQuantity);
        existing.UpdatePriceSnapshot(product.Price);
    }

    public void UpdateItem(Guid itemId, int quantity)
    {
        var item = GetItem(itemId);
        item.UpdateQuantity(quantity, item.Product.StockQuantity);
    }

    public void RemoveItem(Guid itemId) => _items.Remove(GetItem(itemId));

    public void Clear() => _items.Clear();

    private CartItem GetItem(Guid itemId) =>
        _items.FirstOrDefault(x => x.Id == itemId) ?? throw new InvalidOperationException("Cart item not found.");
}
