namespace ECommerceAPI.Application.Features.Cart.Dtos;

internal static class CartMappings
{
    public static CartDto ToDto(this Domain.Entities.Cart cart)
    {
        var items = cart.Items
            .Select(x => new CartItemDto(x.Id, x.ProductId, x.Product.Name, x.Quantity, x.UnitPriceSnapshot, x.UnitPriceSnapshot * x.Quantity))
            .ToList();

        return new CartDto(cart.Id, items, items.Sum(x => x.TotalPrice));
    }
}
