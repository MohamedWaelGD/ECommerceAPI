namespace ECommerceAPI.Domain.Entities;

public class Product
{
    private Product() { }

    private Product(Guid id, string name, string? description, decimal price, int stockQuantity, string? imageUrl)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
        ImageUrl = imageUrl;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }
    public string? ImageUrl { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    public static Product Create(string name, string? description, decimal price, int stockQuantity, string? imageUrl) =>
        new(Guid.NewGuid(), name, description, price, stockQuantity, imageUrl);

    public void Update(string name, string? description, decimal price, int stockQuantity, string? imageUrl, bool isActive)
    {
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
        ImageUrl = imageUrl;
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool HasStock(int quantity) => StockQuantity >= quantity;

    public void ReduceStock(int quantity)
    {
        if (quantity <= 0) throw new InvalidOperationException("Quantity must be greater than zero.");
        if (!HasStock(quantity)) throw new InvalidOperationException("Insufficient product stock.");

        StockQuantity -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }
}
