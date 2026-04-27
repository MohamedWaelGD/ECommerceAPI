namespace ECommerceAPI.Application.Features.Products.Dtos;

public sealed record ProductDto(Guid Id, string Name, string? Description, decimal Price, int StockQuantity, string? ImageUrl, bool IsActive);
