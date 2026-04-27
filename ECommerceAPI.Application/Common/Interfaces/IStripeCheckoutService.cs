using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Application.Common.Interfaces;

public interface IStripeCheckoutService
{
    Task<(string SessionId, string Url)> CreateCheckoutSessionAsync(Order order, CancellationToken cancellationToken);
    StripeWebhookResult HandleWebhook(string payload, string signature);
}

public sealed record StripeWebhookResult(bool IsCheckoutCompleted, string? SessionId, string? PaymentIntentId);
