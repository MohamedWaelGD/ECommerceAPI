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

    public void SetStripeCheckoutSession(string sessionId) => StripeCheckoutSessionId = sessionId;

    public void MarkPaid(string? stripePaymentIntentId)
    {
        if (Status == OrderStatus.Paid) return;

        Status = OrderStatus.Paid;
        PaidAt = DateTime.UtcNow;
        StripePaymentIntentId = stripePaymentIntentId;
    }
}
