using ECommerceAPI.Domain.Enums;

namespace ECommerceAPI.Domain.Entities;

public class Order
{
    private readonly List<OrderItem> _items = [];

    private Order() { }

    private Order(Guid id, Guid userId, string orderNumber, IEnumerable<CartItem> cartItems)
    {
        Id = id;
        UserId = userId;
        OrderNumber = orderNumber;
        Status = OrderStatus.PendingPayment;
        CreatedAt = DateTime.UtcNow;
        _items.AddRange(cartItems.Select(OrderItem.Create));
        TotalAmount = _items.Sum(x => x.TotalPrice);
    }

    public Guid Id { get; private set; }
    public string OrderNumber { get; private set; } = string.Empty;
    public Guid UserId { get; private set; }
    public User User { get; private set; } = default!;
    public OrderStatus Status { get; private set; } = OrderStatus.PendingPayment;
    public decimal TotalAmount { get; private set; }
    public string? StripeCheckoutSessionId { get; private set; }
    public string? StripeCheckoutSessionUrl { get; private set; }
    public DateTime? StripeCheckoutSessionExpiresAt { get; private set; }
    public string? StripePaymentIntentId { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? PaidAt { get; private set; }
    public ICollection<OrderItem> Items => _items;

    public static Order CreateFromCart(Guid userId, IEnumerable<CartItem> cartItems)
    {
        var items = cartItems.ToList();
        if (items.Count == 0) throw new InvalidOperationException("Cart is empty.");

        return new Order(Guid.NewGuid(), userId, $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}", items);
    }

    public bool HasActiveCheckoutSession(DateTime utcNow) =>
        !string.IsNullOrWhiteSpace(StripeCheckoutSessionUrl) &&
        StripeCheckoutSessionExpiresAt is not null &&
        StripeCheckoutSessionExpiresAt > utcNow;

    public bool MatchesCart(IEnumerable<CartItem> cartItems)
    {
        var cart = cartItems
            .Select(x => new { x.ProductId, x.Quantity, UnitPrice = x.UnitPriceSnapshot })
            .OrderBy(x => x.ProductId)
            .ToList();

        var order = _items
            .Select(x => new { x.ProductId, x.Quantity, UnitPrice = x.UnitPrice })
            .OrderBy(x => x.ProductId)
            .ToList();

        return cart.Count == order.Count &&
            cart.Zip(order).All(x =>
                x.First.ProductId == x.Second.ProductId &&
                x.First.Quantity == x.Second.Quantity &&
                x.First.UnitPrice == x.Second.UnitPrice);
    }

    public void SetStripeCheckoutSession(string sessionId, string sessionUrl, DateTime expiresAt)
    {
        StripeCheckoutSessionId = sessionId;
        StripeCheckoutSessionUrl = sessionUrl;
        StripeCheckoutSessionExpiresAt = expiresAt;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Paid) return;
        Status = OrderStatus.Cancelled;
    }

    public void MarkPaid(string? stripePaymentIntentId)
    {
        if (Status == OrderStatus.Paid) return;

        Status = OrderStatus.Paid;
        PaidAt = DateTime.UtcNow;
        StripePaymentIntentId = stripePaymentIntentId;
    }
}
