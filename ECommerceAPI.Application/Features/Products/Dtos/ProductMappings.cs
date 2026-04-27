using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Application.Features.Products.Dtos;

internal static class ProductMappings
{
    public static ProductDto ToDto(this Product product) =>
        new(product.Id, product.Name, product.Description, product.Price, product.StockQuantity, product.ImageUrl, product.IsActive);
}
