using ECommerceAPI.Domain.Enums;

namespace ECommerceAPI.Application.Features.Orders.Dtos;

public sealed record OrderDto(Guid Id, string OrderNumber, OrderStatus Status, decimal TotalAmount, DateTime CreatedAt, DateTime? PaidAt, IReadOnlyCollection<OrderItemDto> Items);

public sealed record OrderItemDto(Guid Id, Guid ProductId, string ProductName, decimal UnitPrice, int Quantity, decimal TotalPrice);
