using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Products.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(string Name, string? Description, decimal Price, int StockQuantity, string? ImageUrl) : IRequest<Result<ProductDto>>;
