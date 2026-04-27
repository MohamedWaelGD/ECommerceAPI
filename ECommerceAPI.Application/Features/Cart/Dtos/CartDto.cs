using ECommerceAPI.Application.Common.Models;

namespace ECommerceAPI.Application.Features.Cart.Dtos;

public sealed record CartDto(Guid Id, PaginatedResponse<CartItemDto> Items, decimal Total);

public sealed record CartItemDto(Guid Id, Guid ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal TotalPrice);
