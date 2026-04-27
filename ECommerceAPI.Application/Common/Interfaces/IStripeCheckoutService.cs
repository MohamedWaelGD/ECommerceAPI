using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Application.Common.Interfaces;

public interface IStripeCheckoutService
{
    Task<StripeCheckoutSession> CreateCheckoutSessionAsync(Order order, CancellationToken cancellationToken);
    StripeWebhookResult HandleWebhook(string payload, string signature);
}

public sealed record StripeCheckoutSession(string SessionId, string Url, DateTime ExpiresAt);
public sealed record StripeWebhookResult(bool IsCheckoutCompleted, string? SessionId, string? PaymentIntentId);
