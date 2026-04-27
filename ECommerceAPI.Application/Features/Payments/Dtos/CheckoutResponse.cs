namespace ECommerceAPI.Application.Features.Payments.Dtos;

public sealed record CheckoutResponse(Guid OrderId, string CheckoutUrl);
