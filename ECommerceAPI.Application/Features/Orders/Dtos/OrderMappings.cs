namespace ECommerceAPI.Application.Features.Orders.Dtos;

internal static class OrderMappings
{
    public static OrderDto ToDto(this Domain.Entities.Order order) => new(
        order.Id,
        order.OrderNumber,
        order.Status,
        order.TotalAmount,
        order.CreatedAt,
        order.PaidAt,
        order.Items.Select(i => new OrderItemDto(i.Id, i.ProductId, i.ProductNameSnapshot, i.UnitPrice, i.Quantity, i.TotalPrice)).ToList());
}
