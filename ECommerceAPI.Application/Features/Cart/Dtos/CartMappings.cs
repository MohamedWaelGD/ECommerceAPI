using ECommerceAPI.Application.Common.Models;

namespace ECommerceAPI.Application.Features.Cart.Dtos;

internal static class CartMappings
{
    public static CartDto ToDto(this Domain.Entities.Cart cart, int page = 1, int pageSize = 100)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var allItems = cart.Items
            .Select(x => new CartItemDto(x.Id, x.ProductId, x.Product.Name, x.Quantity, x.UnitPriceSnapshot, x.UnitPriceSnapshot * x.Quantity))
            .ToList();

        var items = allItems
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var totalPages = (int)Math.Ceiling(allItems.Count / (double)pageSize);
        var paginatedItems = new PaginatedResponse<CartItemDto>(items, page, pageSize, allItems.Count, totalPages);

        return new CartDto(cart.Id, paginatedItems, allItems.Sum(x => x.TotalPrice));
    }
}
