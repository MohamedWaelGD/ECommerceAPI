using ECommerceAPI.Application.Common.Interfaces;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace ECommerceAPI.Infrastructure.Services;

public class StripeCheckoutService(IOptions<StripeOptions> options) : IStripeCheckoutService
{
    private readonly StripeOptions _options = options.Value;

    public async Task<(string SessionId, string Url)> CreateCheckoutSessionAsync(Order order, CancellationToken cancellationToken)
    {
        StripeConfiguration.ApiKey = _options.SecretKey;

        var service = new SessionService();
        var session = await service.CreateAsync(new SessionCreateOptions
        {
            Mode = "payment",
            SuccessUrl = _options.SuccessUrl,
            CancelUrl = _options.CancelUrl,
            ClientReferenceId = order.Id.ToString(),
            Metadata = new Dictionary<string, string> { ["orderId"] = order.Id.ToString() },
            LineItems = order.Items.Select(item => new SessionLineItemOptions
            {
                Quantity = item.Quantity,
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    UnitAmountDecimal = item.UnitPrice * 100,
                    ProductData = new SessionLineItemPriceDataProductDataOptions { Name = item.ProductNameSnapshot }
                }
            }).ToList()
        }, cancellationToken: cancellationToken);

        return (session.Id, session.Url);
    }

    public StripeWebhookResult HandleWebhook(string payload, string signature)
    {
        var stripeEvent = EventUtility.ConstructEvent(payload, signature, _options.WebhookSecret);
        if (stripeEvent.Type != "checkout.session.completed" || stripeEvent.Data.Object is not Session session)
        {
            return new StripeWebhookResult(false, null, null);
        }

        return new StripeWebhookResult(true, session.Id, session.PaymentIntentId);
    }
}
