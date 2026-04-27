using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Common.Models;
using ECommerceAPI.Application.Features.Products.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Products.Queries.GetProducts;

public sealed record GetProductsQuery(int Page = 1, int PageSize = 20) : IRequest<Result<PaginatedResponse<ProductDto>>>;
