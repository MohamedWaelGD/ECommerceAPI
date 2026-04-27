using ECommerceAPI.Application.Common.Interfaces;
using ECommerceAPI.Application.Common.Interfaces.Repositories;
using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Domain.Enums;
using MediatR;

namespace ECommerceAPI.Application.Features.Payments.Commands.HandleStripeWebhook;

public class HandleStripeWebhookCommandHandler(IUnitOfWork unitOfWork, IStripeCheckoutService stripeCheckoutService) : IRequestHandler<HandleStripeWebhookCommand, Result>
{
    public async Task<Result> Handle(HandleStripeWebhookCommand request, CancellationToken cancellationToken)
    {
        var webhook = stripeCheckoutService.HandleWebhook(request.Payload, request.Signature);
        if (!webhook.IsCheckoutCompleted || string.IsNullOrWhiteSpace(webhook.SessionId)) return Result.Success();

        var order = await unitOfWork.Orders.GetByStripeSessionIdWithItemsAsync(webhook.SessionId, cancellationToken);
        if (order is null) return Result.Failure("Order not found for Stripe session.");
        if (order.Status == OrderStatus.Paid) return Result.Success();

        try
        {
            foreach (var item in order.Items)
            {
                var product = await unitOfWork.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId, cancellationToken);
                if (product is null) return Result.Failure($"Product '{item.ProductNameSnapshot}' not found.");

                product.ReduceStock(item.Quantity);
            }

            var cart = await unitOfWork.Carts.GetByUserIdWithItemsAsync(order.UserId, cancellationToken);
            cart?.Clear();
            order.MarkPaid(webhook.PaymentIntentId);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(ex.Message);
        }

        return Result.Success();
    }
}
