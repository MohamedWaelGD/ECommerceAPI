using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Products.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<Result<ProductDto>>;
