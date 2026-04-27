using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Products.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(Guid Id, string Name, string? Description, decimal Price, int StockQuantity, string? ImageUrl, bool IsActive) : IRequest<Result<ProductDto>>;
