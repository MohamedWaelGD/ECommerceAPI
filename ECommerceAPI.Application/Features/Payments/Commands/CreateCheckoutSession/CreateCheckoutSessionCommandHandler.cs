using ECommerceAPI.Application.Common.Interfaces;
using ECommerceAPI.Application.Common.Interfaces.Repositories;
using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Payments.Dtos;
using ECommerceAPI.Domain.Entities;
using MediatR;

namespace ECommerceAPI.Application.Features.Payments.Commands.CreateCheckoutSession;

public class CreateCheckoutSessionCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser, IStripeCheckoutService stripeCheckoutService)
    : IRequestHandler<CreateCheckoutSessionCommand, Result<CheckoutResponse>>
{
    public async Task<Result<CheckoutResponse>> Handle(CreateCheckoutSessionCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        if (userId is null) return Result<CheckoutResponse>.Failure("User is not authenticated.");

        var cart = await unitOfWork.Carts.GetOrCreateByUserIdAsync(userId.Value, cancellationToken);
        if (cart.Items.Count == 0) return Result<CheckoutResponse>.Failure("Cart is empty.");

        foreach (var item in cart.Items)
        {
            if (!item.Product.IsActive) return Result<CheckoutResponse>.Failure($"Product '{item.Product.Name}' is inactive.");
            if (!item.Product.HasStock(item.Quantity)) return Result<CheckoutResponse>.Failure($"Product '{item.Product.Name}' does not have enough stock.");
        }

        var order = Order.CreateFromCart(userId.Value, cart.Items);
        unitOfWork.Orders.Add(order);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var session = await stripeCheckoutService.CreateCheckoutSessionAsync(order, cancellationToken);
        order.SetStripeCheckoutSession(session.SessionId);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CheckoutResponse>.Success(new CheckoutResponse(order.Id, session.Url));
    }
}
